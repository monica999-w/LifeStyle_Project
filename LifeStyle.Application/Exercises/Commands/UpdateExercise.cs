using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;

using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record UpdateExercise(int ExerciseId, string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class UpdateExerciseHandler : IRequestHandler<UpdateExercise, ExerciseDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateExerciseHandler(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("UpdateExerciseHandler instance created.");
        }

        public async Task<ExerciseDto> Handle(UpdateExercise request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateExercise command...");

            try
            {
                Log.Information("Updating exercise with Id {ExerciseId}", request.ExerciseId);
                var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);
                if (exercise == null)
                {
                    Log.Warning("Exercise not found: ID={ExerciseId}", request.ExerciseId);
                    throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found");
                }

                _mapper.Map(request, exercise);

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.ExerciseRepository.Update(exercise);
                await _unitOfWork.SaveAsync();

                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();

                Log.Information("Exercise updated successfully: ID={ExerciseId}", request.ExerciseId);

                return _mapper.Map<ExerciseDto>(exercise);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Exercise not found");
                throw;
            }
            catch (DataValidationException ex)
            {
                Log.Error(ex, "Data validation error");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update exercise");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to update exercise", ex);
            }
        }
    }
}
