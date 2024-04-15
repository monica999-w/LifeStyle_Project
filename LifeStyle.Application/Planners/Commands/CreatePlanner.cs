using LifeStyle.Aplication.Interfaces;
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
        private readonly IPlannerRepository _plannerRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IRepository<Meal> _mealRepository;
        private readonly IRepository<Exercise> _exerciseRepository;

        public CreatePlannerHandler(IPlannerRepository plannerRepository, IRepository<UserProfile> userRepository, IRepository<Meal> mealRepository, IRepository<Exercise> exerciseRepository)
        {
            _plannerRepository = plannerRepository;
            _userRepository = userRepository;
            _mealRepository = mealRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<PlannerDto> Handle(CreatePlanner request, CancellationToken cancellationToken)
        {
           
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            
            var meals = new List<Meal>();
            foreach (var mealId in request.MealIds)
            {
                var meal = await _mealRepository.GetById(mealId);
                if (meal == null)
                    throw new Exception($"Meal with ID {mealId} not found");

                meals.Add(meal);
            }

            
            var exercises = new List<Exercise>();
            foreach (var exerciseId in request.ExerciseIds)
            {
                var exercise = await _exerciseRepository.GetById(exerciseId);
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

            await _plannerRepository.AddPlanner(planner);

            
            return new PlannerDto
            {
                Profile = UserDto.FromUser(planner.Profile),
                Meals = planner.Meals?.Select(MealDto.FromMeal).ToList(),
                Exercises = planner.Exercises?.Select(ExerciseDto.FromExercise).ToList()
            };
        }

    }
}

