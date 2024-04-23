using LifeStyle.Application.Responses;
using LifeStyle.Models.Planner;


namespace LifeStyle.Application.Planners.Responses
{
    public class PlannerDto
    {
        public UserDto Profile { get; set; }
        public List<MealDto>? Meals { get; set; }
        public List<ExerciseDto>?Exercises { get; set; }

        public static PlannerDto FromPlanner(Planner planner)
        {
            return new PlannerDto
            {
                Profile = UserDto.FromUser(planner.Profile),
                Meals = planner.Meals?.Select(MealDto.FromMeal).ToList(),
                Exercises = planner.Exercises?.Select(ExerciseDto.FromExercise).ToList()
            };
        }
    }
}
