using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record CreatePlanner(int UserId, List<int> MealIds, List<int> ExerciseIds) : IRequest<Planner>;

    public class CreatePlannerHandler : IRequestHandler<CreatePlanner, Planner>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlannerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("CreatePlannerHandler instance created.");
        }

        public async Task<Planner> Handle(CreatePlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling CreatePlanner command...");
            try
            {
                Log.Information("Creating Planner..");
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("Planner with User Id not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                var planner = new Planner(user);

                // Asocierea meselor
                foreach (var mealId in request.MealIds)
                {
                    var meal = await _unitOfWork.MealRepository.GetById(mealId);
                    if (meal == null)
                    {
                        Log.Warning("Meal not found: ID={MealId}", mealId);
                        throw new NotFoundException($"Meal with ID {mealId} not found");
                    }
                    planner.AddMeal(meal);
                }

                // Asocierea exercițiilor
                foreach (var exerciseId in request.ExerciseIds)
                {
                    var exercise = await _unitOfWork.ExerciseRepository.GetById(exerciseId);
                    if (exercise == null)
                    {
                        Log.Warning("Exercise not found: ID={ExerciseId}", exerciseId);
                        throw new NotFoundException($"Exercise with ID {exerciseId} not found");
                    }
                    planner.AddExercise(exercise);
                }

                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.PlannerRepository.AddPlanner(planner);
                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Planner created successfully");

                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create planner", ex);
            }
        }
    }
}