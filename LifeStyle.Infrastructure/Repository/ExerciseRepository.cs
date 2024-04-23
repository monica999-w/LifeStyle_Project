using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LifeStyle.Aplication.Logic
{

    public class ExerciseRepository : IRepository<Exercise>
    {

        private readonly LifeStyleContext _lifeStyleContext;

        public ExerciseRepository(LifeStyleContext lifeStyleContext)
        {
            _lifeStyleContext = lifeStyleContext;
        }

        public async Task<IEnumerable<Exercise>> GetAll()
        {
           
            return await _lifeStyleContext.Exercises
                .ToListAsync();
        }

        public async Task<Exercise> Add(Exercise entity)
        {
            _lifeStyleContext.Exercises.Add(entity);
            await _lifeStyleContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Exercise> Remove(Exercise entity)
        {
          
            var existingExercise = await GetById(entity.ExerciseId);
            if (existingExercise != null)
            {
                _lifeStyleContext.Exercises.Remove(existingExercise);
            }
            else
            {
                throw new Exception("Exercise not found");
            }
            return entity;
        }

        public async Task<Exercise> Update(Exercise entity)
        {
            var existingExercise = await GetById(entity.ExerciseId);
            if (existingExercise != null)
            {

                existingExercise.Name = entity.Name;
                existingExercise.DurationInMinutes = entity.DurationInMinutes;
                existingExercise.Type = entity.Type;
            }
            else
            {
                throw new Exception("Exercise not found");
            }
            return entity;
        }

        public async Task<Exercise?> GetById(int id)
        {
          
            var exercise = await _lifeStyleContext.Exercises
                .FirstOrDefaultAsync(e => e.ExerciseId == id);
          
            return exercise;
        }

        public async Task<Exercise> GetByName(string name)
        {
            var exercise = await _lifeStyleContext.Exercises
                .FirstOrDefaultAsync(e => e.Name == name);

            return exercise;
        }


        public int GetLastId()
        {
            if (_lifeStyleContext.Exercises.Any())
            {
                return _lifeStyleContext.Exercises.Max(m => m.ExerciseId);
            }
            else
            {
                return 0;
            }
        }
    }
}