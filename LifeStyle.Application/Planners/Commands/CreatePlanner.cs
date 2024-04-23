using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using MediatR;


namespace LifeStyle.Application.Planners.Commands
{
    public record CreatePlanner(int UserId, List<int> MealIds, List<int> ExerciseIds) : IRequest<PlannerDto>;

    public class CreatePlannerHandler : IRequestHandler<CreatePlanner, PlannerDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlannerHandler(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public async Task<PlannerDto> Handle(CreatePlanner request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserProfileRepository.GetById(request.UserId);
                if (user == null)
                    throw new Exception($"User with ID {request.UserId} not found");


                var meals = new List<Meal>();
                foreach (var mealId in request.MealIds)
                {
                    var meal = await _unitOfWork.MealRepository.GetById(mealId);
                    if (meal == null)
                        throw new Exception($"Meal with ID {mealId} not found");

                    meals.Add(meal);
                }


                var exercises = new List<Exercise>();
                foreach (var exerciseId in request.ExerciseIds)
                {
                    var exercise = await _unitOfWork.ExerciseRepository.GetById(exerciseId);
                    if (exercise == null)
                        throw new Exception($"Exercise with ID {exerciseId} not found");

                    exercises.Add(exercise);
                }


                var planner = new Planner(user);

                foreach (var meal in meals)
                {
                    planner.AddMeal(meal);
                }

                foreach (var exercise in exercises)
                {
                    planner.AddExercise(exercise);
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.PlannerRepository.AddPlanner(planner);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return new PlannerDto
                {
                    Profile = UserDto.FromUser(planner.Profile),
                    Meals = planner.Meals.Select(MealDto.FromMeal).ToList(),
                    Exercises = planner.Exercises?.Select(ExerciseDto.FromExercise).ToList()
                };
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create planner", ex);
            }
        }

    }
}

