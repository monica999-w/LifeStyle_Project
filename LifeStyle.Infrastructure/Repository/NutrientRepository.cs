using LifeStyle.Aplication.Interfaces;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace LifeStyle.Infrastructure.Repository
{
    public class NutrientRepository : IRepository<Nutrients>
    {
        private readonly LifeStyleContext _lifeStyleContext;

        public NutrientRepository(LifeStyleContext lifeStyleContext)
        {

            _lifeStyleContext = lifeStyleContext;

        }

        public async Task<List<Nutrients>> GetAll()
        {

            return await _lifeStyleContext.Nutrients
                .Take(100)
                .ToListAsync();
        }

        public async Task<Nutrients> Add(Nutrients entity)
        {
            _lifeStyleContext.Nutrients.Add(entity);
            await _lifeStyleContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Nutrients> Remove(Nutrients entity)
        {

            var existingNutrient = await GetById(entity.NutrientId);
            if (existingNutrient != null)
            {
                _lifeStyleContext.Nutrients.Remove(existingNutrient);
            }
            else
            {
                throw new Exception("Nutrient not found");
            }
            return entity;
        }

        public async Task<Nutrients> Update(Nutrients entity)
        {
            var existingNutrient = await GetById(entity.NutrientId);
            if (existingNutrient != null)
            {
                existingNutrient.Protein=entity.Protein;
                existingNutrient.Calories=entity.Calories;
                existingNutrient.Carbohydrates=entity.Carbohydrates;
                existingNutrient.Fat=entity.Fat;
            }
            else
            {
                throw new Exception("Nutrient not found");
            }
            return entity;
        }

        public async Task<Nutrients?> GetById(int id)
        {

            var nutrient = await _lifeStyleContext.Nutrients
                .FirstOrDefaultAsync(e => e.NutrientId == id);

            return nutrient;
        }


        public int GetLastId()
        {
            if (_lifeStyleContext.Nutrients.Any())
            {
                return _lifeStyleContext.Nutrients.Max(m => m.NutrientId);
            }
            else
            {
                return 0;
            }
        }

        public Task<Nutrients> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
