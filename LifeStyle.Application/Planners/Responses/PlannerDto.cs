using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using System.Collections.ObjectModel;


namespace LifeStyle.Application.Planners.Responses
{
    public class PlannerDto
    {
        public int PlannerId { get; set; }
        public int ProfileId { get; set; }
        public List<int> MealIds { get; set; }
        public List<int> ExerciseIds { get; set; }
    }
}
