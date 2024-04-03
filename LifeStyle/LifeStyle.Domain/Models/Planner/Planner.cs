using LifeStyle.LifeStyle.Domain.Models.Exercises;
using LifeStyle.LifeStyle.Domain.Models.Users;
using LifeStyle.LifeStyle.Domain.Models.Meal;

namespace LifeStyle.Models.Planner
{
    public class Planner
    {
        public UserProfile Profile { get; set; }
        public List<Meal>? Meals { get; set; }
        public List<Exercise>? Exercises { get; set; }

        public Planner(UserProfile profile)
        {
            Profile = profile;
            Meals = new List<Meal>();
            Exercises = new List<Exercise>();
        }

        public void AddMeal(Meal meal)
        {
            Meals?.Add(meal);
        }


        public void AddExercise(Exercise exercise)
        {
            Exercises?.Add(exercise);
        }

        
        public void RemoveMeal(Meal meal)
        {
            Meals?.Remove(meal);
        }

        
        public void RemoveExercise(Exercise exercise)
        {
            Exercises?.Remove(exercise);
        }

    }
}
