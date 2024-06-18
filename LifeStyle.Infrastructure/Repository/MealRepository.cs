using LifeStyle.Domain.Enums;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Application.Exercises.Responses;

namespace LifeStyle.Aplication.Logic
{

    public class MealRepository : IRepository<Meal>
    {

        private readonly LifeStyleContext _lifeStyleContext;

        public MealRepository(LifeStyleContext lifeStyleContext)
        {

            _lifeStyleContext = lifeStyleContext;

        }

        public async Task<List<Meal>> GetAll()
        {

            return await _lifeStyleContext.Meals
                  .Include(meal => meal.Nutrients)
                  .ToListAsync();
        }


        public async Task<Meal> Add(Meal entity)
        {
            _lifeStyleContext.Meals.Add(entity);
            await _lifeStyleContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Meal> Remove(Meal entity)
        {

            var existingMeal = await GetById(entity.MealId);
            if (existingMeal != null)
            {
                _lifeStyleContext.Meals.Remove(existingMeal);
            }
            else
            {
                throw new Exception("Meal not found");
            }
            return entity;
        }

        public async Task<Meal> Update(Meal entity)
        {
            var existingMeal = await GetById(entity.MealId);
            if (existingMeal != null)
            {

                existingMeal.MealName = entity.MealName;
                existingMeal.MealType = entity.MealType;
                existingMeal.Nutrients = entity.Nutrients;
               
            }
            else
            {
                throw new Exception("Meal not found");
            }
            return entity;
        }

        public async Task<Meal?> GetById(int id)
        {
          
            var meal = await _lifeStyleContext.Meals
                .Include(m => m.Nutrients)
                .FirstOrDefaultAsync(e => e.MealId == id);

            return meal;
        }

        public int GetLastId()
        {
            if (_lifeStyleContext.Meals.Any())
            {
                return _lifeStyleContext.Meals.Max(m => m.MealId);
            }
            else
            {
                return 0;
            }
        }

        public async Task<Meal> GetByName(string name)
        {
            var meal = await _lifeStyleContext.Meals
                .FirstOrDefaultAsync(e => e.MealName == name);

            return meal;
        }

        public async Task<IEnumerable<Meal>> SearchAsync(string searchTerm)
        {
            return await _lifeStyleContext.Meals
                .Where(m => m.MealName.Contains(searchTerm) || m.Ingredients.Any(i => i.Contains(searchTerm)))
                .ToListAsync();
        }


    }
}