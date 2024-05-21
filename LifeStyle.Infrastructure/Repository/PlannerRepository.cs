using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Models.Planner;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LifeStyle.Aplication.Logic
{
    public class PlannerRepository : IPlannerRepository
    {
        private readonly LifeStyleContext _lifeStyleContext;

        public PlannerRepository(LifeStyleContext lifeStyleContext)
        {
            _lifeStyleContext = lifeStyleContext;
        }

        public  async Task<Planner> AddPlanner(Planner planner)
        {
            _lifeStyleContext.Planners.Add(planner);
            await _lifeStyleContext.SaveChangesAsync();
            return planner;
        }

        public async Task<Planner> RemovePlanner(Planner planner)
        {

            var existingMeal = await GetPlannerByUser(planner.Profile);
            if (existingMeal != null)
            {
                _lifeStyleContext.Planners.Remove(existingMeal);
            }
            else
            {
                throw new Exception("Meal not found");
            }
            return planner;
        }

        public async Task<Planner?> GetPlannerByUser(UserProfile profile)
        {
            var planner = await _lifeStyleContext.Planners
                .FirstOrDefaultAsync(p => p.Profile == profile);
            return planner;
        }
        public async Task<Planner?> GetPlannerById(int id)
        {

            var planner = await _lifeStyleContext.Planners
                .FirstOrDefaultAsync(e => e.PlannerId == id);

            return planner;
        }

        public async Task<IEnumerable<Planner>> GetAll()
        {
           
            return await _lifeStyleContext.Planners
                 .Include(meal => meal.Meals)
                 .Include(exercise => exercise.Exercises)
                 .Include(user => user.Profile)
                .ToListAsync();
        }


        public async Task<Planner?> UpdatePlannerAsync(Planner planner)
        {

            var existingPlanner = await GetPlannerByUser(planner.Profile);
            if (existingPlanner != null)
            {
                existingPlanner.Meals = planner.Meals;
                existingPlanner.Exercises = planner.Exercises;

            }
            else
            {
                throw new Exception("Planner not found");
            }
            return planner;

        }

       public async Task RemoveMealAsync(Meal meal)
        {
            var plannerContainingMeal = await _lifeStyleContext.Planners
                .Include(p => p.Meals)
                .FirstOrDefaultAsync(p => p.Meals.Any(m => m.MealId == meal.MealId));

            if (plannerContainingMeal != null)
            {
                plannerContainingMeal?.Meals.Remove(meal);
            }
        }

        public async Task RemoveExerciseAsync(Exercise exercise)
        {
            var plannerContainingExercise = await _lifeStyleContext.Planners
                .Include(p => p.Exercises)
                .FirstOrDefaultAsync(p => p.Exercises.Any(e => e.ExerciseId == exercise.ExerciseId));

            if (plannerContainingExercise != null)
            {
                plannerContainingExercise?.Exercises.Remove(exercise);
            }
        }
       
    }
}

