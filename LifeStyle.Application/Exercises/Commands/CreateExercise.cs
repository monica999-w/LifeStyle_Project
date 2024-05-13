using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record CreateExercise(string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class CreateExerciseHandler : IRequestHandler<CreateExercise, ExerciseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateExerciseHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("CreateExerciseHandler instance created.");
        }

        public async Task<ExerciseDto> Handle(CreateExercise request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreateExercise command...");
            try
            {
                Log.Information("Creating exercise: Name={Name}, DurationInMinutes={DurationInMinutes}, Type={Type}", request.Name, request.DurationInMinutes, request.Type);
                var existingExercise = await _unitOfWork.ExerciseRepository.GetByName(request.Name);
                if (existingExercise != null)
                {
                    Log.Warning("Exercise with the same name already exists: Name={Name}", request.Name);
                    throw new AlreadyExistsException("Exercise already exists");
                }

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();
                var newExercise = _mapper.Map<Exercise>(request);
                


                Log.Information("Adding exercise to repository...");
                await _unitOfWork.ExerciseRepository.Add(newExercise);
                await _unitOfWork.SaveAsync();

                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Exercise created successfully: ID={ExerciseId}", newExercise.ExerciseId);


                return _mapper.Map<ExerciseDto>(newExercise);
            }
            catch (AlreadyExistsException ex)
            {
                Log.Error(ex, "Failed to create exercise: Exercise already exists");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create exercise");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create exercise", ex);
            }
           
        }
    }
}
