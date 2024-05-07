using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateExercise(string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class CreateExerciseHandler : IRequestHandler<CreateExercise, ExerciseDto>
    {
        private readonly IUnitOfWork _unitOfWork;


        public CreateExerciseHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseDto> Handle(CreateExercise request, CancellationToken cancellationToken)
        {
            try
            {
                var existingExercise = await _unitOfWork.ExerciseRepository.GetByName(request.Name);
                if (existingExercise != null)
                {
                    throw new AlreadyExistsException("Exercise already exists");
                }
                await _unitOfWork.BeginTransactionAsync();
                var newExercise = new Exercise
                {
                    Name = request.Name,
                    DurationInMinutes = request.DurationInMinutes,
                    Type = request.Type
                };

               

                await _unitOfWork.ExerciseRepository.Add(newExercise);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                
                return ExerciseDto.FromExercise(newExercise);
            }
            catch (AlreadyExistsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to create exercise", ex);
            }
           
        }
    }
}
