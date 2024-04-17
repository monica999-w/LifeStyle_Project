using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using MediatR;


namespace LifeStyle.Application.Planners.Commands
{
    public record UpdatePlanner(int UserId, List<int> MealIds = null, List<int> ExerciseIds = null) : IRequest<bool>;

    public class UpdatePlannerHandler : IRequestHandler<UpdatePlanner, bool>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IRepository<Meal> _mealRepository;
        private readonly IRepository<Exercise> _exerciseRepository;

        public UpdatePlannerHandler(IPlannerRepository plannerRepository, IRepository<UserProfile> userRepository, IRepository<Meal> mealRepository, IRepository<Exercise> exerciseRepository)
        {
            _plannerRepository = plannerRepository;
            _userRepository = userRepository;
            _mealRepository = mealRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<bool> Handle(UpdatePlanner request, CancellationToken cancellationToken)
        {
           
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            var meals = new List<Meal>();
            if (request.MealIds != null)
            {
                foreach (var mealId in request.MealIds)
                {
                    var meal = await _mealRepository.GetById(mealId);
                    if (meal == null)
                        throw new Exception($"Meal with ID {mealId} not found");

                    meals.Add(meal);
                }
            }

            var exercises = new List<Exercise>();
            if (request.ExerciseIds != null)
            {
                foreach (var exerciseId in request.ExerciseIds)
                {
                    var exercise = await _exerciseRepository.GetById(exerciseId);
                    if (exercise == null)
                        throw new Exception($"Exercise with ID {exerciseId} not found");

                    exercises.Add(exercise);
                }
            }

            var planner = await _plannerRepository.GetPlannerByUser(user);
            if (planner == null)
                throw new Exception($"Planner not found for user with ID {user.ProfileId}");

            
            if (meals.Any())
            {
                planner.Meals = meals;
            }

            if (exercises.Any())
            {
                planner.Exercises = exercises;
            }

            await _plannerRepository.UpdatePlannerAsync(planner);

            return true;
        }

    }
}
