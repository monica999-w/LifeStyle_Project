using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Planners.Commands
{
    public record UpdatePlanner(int UserId, List<int>? MealIds = null, List<int>? ExerciseIds = null) : IRequest<bool>;

    public class UpdatePlannerHandler : IRequestHandler<UpdatePlanner, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public UpdatePlannerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            

            Log.Information("UpdatePlannerHandler instance created.");
          
        }

        public async Task<bool> Handle(UpdatePlanner request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdatePlanner command...");
            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                {
                    Log.Warning("Planner with User Id not found: ID={UserId}", request.UserId);
                    throw new NotFoundException($"User with ID {request.UserId} not found");
                }

                var meals = new List<Meal>();
                if (request.MealIds != null)
                {
                    foreach (var mealId in request.MealIds)
                    {
                        var meal = await _unitOfWork.MealRepository.GetById(mealId);
                        if (meal == null)
                        {
                            Log.Warning("Planner with Meal Id not found: ID={MealIds}", request.MealIds);
                            throw new NotFoundException($"Meal with ID {mealId} not found");
                        }
                        meals.Add(meal);
                    }
                }

                var exercises = new List<Exercise>();
                if (request.ExerciseIds != null)
                {
                    foreach (var exerciseId in request.ExerciseIds)
                    {
                        var exercise = await _unitOfWork.ExerciseRepository.GetById(exerciseId);
                        if (exercise == null)
                        {
                            Log.Warning("Planner with Exercise Id not found: ID={ExerciseIds}", request.ExerciseIds);
                            throw new NotFoundException($"Exercise with ID {exerciseId} not found");
                        }
                        exercises.Add(exercise);
                    }
                }

                var planner = await _unitOfWork.PlannerRepository.GetPlannerByUser(user);
                if (planner == null)
                {
                    throw new NotFoundException($"Planner not found for user with ID {user.ProfileId}");
                }

                if (meals.Count != 0)
                {
                    planner.Meals = meals;
                }

                if (exercises.Count != 0)
                {
                    planner.Exercises = exercises;
                }
                Log.Information("Starting transaction...");
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PlannerRepository.UpdatePlannerAsync(planner);
                await _unitOfWork.SaveAsync();
                Log.Information("Committing transaction...");
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Planner update successfully");

                return true;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Not found exception");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update planner");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to update planner", ex);
            }
        }
    }
}
