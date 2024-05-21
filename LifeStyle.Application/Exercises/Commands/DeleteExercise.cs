using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record DeleteExercise(int ExerciseId) : IRequest<Exercise>;

    public class DeleteExerciseHandler : IRequestHandler<DeleteExercise, Exercise>
    {

        private readonly IUnitOfWork _unitOfWork;
       
        public DeleteExerciseHandler( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
            Log.Information("DeleteExerciseHandler instance created.");
        }

        public async Task<Exercise> Handle(DeleteExercise request, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeleteExercise command...");

            try
            {
                Log.Information("Deleting exercise with Id {ExerciseId}", request.ExerciseId);
                var exercise = await _unitOfWork.ExerciseRepository.GetById(request.ExerciseId);

                if (exercise == null)
                {
                    Log.Warning("Exercise not found: ID={ExerciseId}", request.ExerciseId);
                    throw new NotFoundException("Exercise not found");
                }

              
                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.ExerciseRepository.Remove(exercise);
                await _unitOfWork.SaveAsync();

                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();

                Log.Information("Exercise deleted successfully: ID={ExerciseId}", request.ExerciseId);

                return exercise;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Exercise not found");
                throw new NotFoundException("Exercise not found", ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete exercise");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete exercise", ex);
            }
        }
    }
}
