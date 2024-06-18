using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record RemoveExerciseFromPlanner(int PlannerId, int ExerciseId) : IRequest<Planner>;

    public class RemoveExerciseFromPlannerHandler : IRequestHandler<RemoveExerciseFromPlanner, Planner>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveExerciseFromPlannerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("RemoveExerciseFromPlannerHandler instance created.");
        }

        public async Task<Planner> Handle(RemoveExerciseFromPlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling RemoveExerciseFromPlanner command...");

            try
            {
                var planner = await _unitOfWork.PlannerRepository.GetPlannerById(request.PlannerId);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner with ID {request.PlannerId} not found");
                }

                if (planner.Exercises == null)
                {
                    throw new InvalidOperationException($"Exercises collection is not initialized for planner with ID {request.PlannerId}");
                }

                var exercise = planner.Exercises.FirstOrDefault(e => e.ExerciseId == request.ExerciseId);
                if (exercise == null)
                {
                    throw new NotFoundException($"Exercise with ID {request.ExerciseId} not found in planner");
                }

                planner.Exercises.Remove(exercise);
                await _unitOfWork.SaveAsync();

                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to remove exercise from planner");
                throw new Exception("Failed to remove exercise from planner", ex);
            }
        }
    }

}
