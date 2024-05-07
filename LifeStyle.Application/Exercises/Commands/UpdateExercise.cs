using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record UpdateExercise(int ExerciseId, string Name, int DurationInMinutes, ExerciseType Type) : IRequest<ExerciseDto>;

    public class UpdateExerciseHandler : IRequestHandler<UpdateExercise, ExerciseDto>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateExerciseHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseDto> Handle(UpdateExercise request, CancellationToken cancellationToken)
        {

            try
            {
                var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);
                if (exercise == null)
                {
                    throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found");
                }

                exercise.Name = request.Name;
                exercise.DurationInMinutes = request.DurationInMinutes;
                exercise.Type = request.Type;

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.ExerciseRepository.Update(exercise);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return ExerciseDto.FromExercise(exercise);
            }
            catch (NotFoundException ex)
            {
                
                throw ;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Fail update exercise", ex);
            }
            
        }
    }
}
