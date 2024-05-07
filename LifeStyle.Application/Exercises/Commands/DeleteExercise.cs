using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record DeleteExercise(int ExerciseId) : IRequest<Unit>;

    public class DeleteExerciseHandler : IRequestHandler<DeleteExercise, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;


        public DeleteExerciseHandler( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteExercise request, CancellationToken cancellationToken)
        {

            try
            {
                var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);

                if (exercise == null)
                {
                    throw new NotFoundException("Exercise not found");
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.ExerciseRepository.Remove(exercise);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
               
                throw new NotFoundException("Exercise not found", ex);
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to delete exercise", ex);
            }
        }
    }
}
