using LifeStyle.Enums;
using LifeStyle.Interfaces;
using LifeStyle.Models.Exercises;
using LifeStyle.Models.Meal;
using LifeStyle.Models.Planner;
using LifeStyle.Models.User;
using System.Reflection;

namespace LifeStyle.Logic
{
    public class PlannerRepository : IPlannerRepository
    {
        private readonly List<Planner> _planners;

        public PlannerRepository()
        {
            var userProfile1 = new UserProfile(1, "user1@example.com", "123456789", 170.0, 70.0);
            var userProfile2 = new UserProfile(2, "user2@example.com", "987654321", 160.0, 60.0);

            var meal1 = new Meal(1, "Breakfast", MealType.Breakfast, new Nutrients(144,24,12,10));
            var meal2 = new Meal(2, "Lunch", MealType.Lunch, new Nutrients(243,23,56,42));
            var meal3 = new Meal(3, "Dinner", MealType.Dinner, new Nutrients(234,43,24,24));

            var exercise1 = new Exercise(1, "Running", 30, ExerciseType.Cardio);
            var exercise2 = new Exercise(2, "Weightlifting", 45, ExerciseType.Yoga);

            var planner1 = new Planner(userProfile1);
            planner1.AddMeal(meal1);
            planner1.AddMeal(meal2);
            planner1.AddMeal(meal3);
            planner1.AddExercise(exercise1);
            planner1.AddExercise(exercise2);

            var planner2 = new Planner(userProfile2);
            planner2.AddMeal(meal1);
            planner2.AddMeal(meal2);
            planner2.AddMeal(meal3);
            planner2.AddExercise(exercise1);
            planner2.AddExercise(exercise2);

            _planners?.Add(planner1);
            _planners?.Add(planner2);
        }


        public Task AddPlanner(Planner planner)
        {
            _planners?.Add(planner);
            return Task.CompletedTask;
        }

        public Task RemovePlanner(Planner planner)
        {
            _planners?.Remove(planner);
            return Task.CompletedTask;
        }

        public Task<Planner> GetPlannerByUser(UserProfile profile)
        {
            return Task.FromResult(_planners.FirstOrDefault(p => p.Profile == profile));
        }

        public Task UpdatePlannerAsync(Planner planner)
        {
            
            var existingPlanner = _planners.FirstOrDefault(p => p.Profile == planner.Profile);
            if (existingPlanner != null)
            {
                existingPlanner.Meals = planner.Meals;
                existingPlanner.Exercises = planner.Exercises;
            }
            return Task.CompletedTask;
        }
    }
}

