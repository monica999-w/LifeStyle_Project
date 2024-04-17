using LifeStyle.Domain.Enums;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain;
using LifeStyle.Domain.Models.Meal;




namespace LifeStyle.Aplication.Logic
{
    public class MealRepository : IRepository<Meal>
    {

        private readonly List<Meal> _meals = new List<Meal>();

        public MealRepository()
        {

            _meals.Add(new Meal(1, "Oatmeal", MealType.Breakfast, new Nutrients(1,144.0, 5.0, 30.0, 2.5)));
            _meals.Add(new Meal(2, "Chicken Salad", MealType.Lunch, new Nutrients(2,255.1, 25.0, 10.0, 15.0)));

        }

        public async Task<IEnumerable<Meal>> GetAll()
        {
            await Task.Delay(0);
            return _meals;
        }

        public async Task Add(Meal entity)
        {
            await Task.Delay(0);
            _meals.Add(entity);
        }

        public async Task Remove(Meal entity)
        {
            await Task.Delay(0);
            var existingMeal = await GetById(entity.MealId);
            if (existingMeal != null)
            {
                _meals.Remove(existingMeal);
            }
            else
            {
                throw new KeyNotFoundException("Meal not found");
            }
        }

        public async Task Update(Meal entity)
        {
            var existingMeal = await GetById(entity.MealId);
            if (existingMeal != null)
            {

                existingMeal.Name = entity.Name;
                existingMeal.MealType = entity.MealType;
                existingMeal.Nutrients = entity.Nutrients;
            }
            else
            {
                throw new KeyNotFoundException("Meal not found");
            }
        }

        public async Task<Meal?> GetById(int id)
        {
            await Task.Delay(0);
            var meal = _meals.FirstOrDefault(m => m.MealId == id);
            if (meal == null)
            {
                throw new KeyNotFoundException("Meal not found");
            }
            return meal;
        }

        public int GetLastId()
        {
            if (_meals.Any())
            {
                return _meals.Max(m => m.MealId);
            }
            else
            {
                return 0;
            }
        }
    }
}