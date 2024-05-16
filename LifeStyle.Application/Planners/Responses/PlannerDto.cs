using LifeStyle.Application.Responses;
using LifeStyle.Models.Planner;
using System.Collections.ObjectModel;


namespace LifeStyle.Application.Planners.Responses
{
    public class PlannerDto
    {
        public int PlannerId { get; set; }
        public UserDto Profile { get; set; }
        public Collection<MealDto>? Meals { get; set; }
        public Collection<ExerciseDto>?Exercises { get; set; }

        
    }
}
