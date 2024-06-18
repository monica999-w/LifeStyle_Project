using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Models.Planner;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record CreatePlanner(string Email, DateTime Date, List<int> MealIds, List<int> ExerciseIds) : IRequest<Planner>;

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
            Log.Information("Handling CreateOrUpdatePlanner command...");
            try
            {
                Log.Information("Creating or updating Planner..");
                var user = await _unitOfWork.UserProfileRepository.GetByName(request.Email);
                if (user == null)
                {
                    Log.Warning("Planner with User Email not found: Email={Email}", request.Email);
                    throw new NotFoundException($"User with Email {request.Email} not found");
                }

                var planner = await _unitOfWork.PlannerRepository.GetPlannerByDate(user.ProfileId, request.Date);
                if (planner == null)
                {
                    planner = new Planner
                    {
                        Profile = user,
                        Date = request.Date,
                        Meals = new List<Meal>(),
                        Exercises = new List<Exercise>()
                    };
                    await _unitOfWork.PlannerRepository.AddPlanner(planner);
                }

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

                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Planner created or updated successfully");

                return planner;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create or update planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create or update planner", ex);
            }
        }
    }
}
