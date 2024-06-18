using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Models.Planner
{
    public class Planner
    {
        [Key]
        public int PlannerId { get; set; }
        public UserProfile? Profile { get; set; } 
        public ICollection<Meal>? Meals { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
        public DateTime Date { get; set; }

        public Planner() { }

        public Planner(UserProfile profile )
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
