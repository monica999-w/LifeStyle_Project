using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using LifeStyle.Domain.Exception;

namespace LifeStyle.Aplication.Logic
{
    public class ExerciseRepository : IRepository<Exercise>
    {
        private readonly LifeStyleContext _lifeStyleContext;

        public ExerciseRepository(LifeStyleContext lifeStyleContext)
        {
            _lifeStyleContext = lifeStyleContext;
        }

        public async Task<List<Exercise>> GetAll()
        {
            try
            {
                return await _lifeStyleContext.Exercises.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercises.", ex);
            }
        }

        public async Task<Exercise> Add(Exercise entity)
        {
            try
            {
                _lifeStyleContext.Exercises.Add(entity);
                await _lifeStyleContext.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new AlreadyExistsException("Exercise with the same name already exists.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the exercise.", ex);
            }
        }

        public async Task<Exercise> Remove(Exercise entity)
        {
            try
            {
                var existingExercise = await GetById(entity.ExerciseId);
                if (existingExercise != null)
                {
                    _lifeStyleContext.Exercises.Remove(existingExercise);
                    await _lifeStyleContext.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException("Exercise not found");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the exercise.", ex);
            }
        }

        public async Task<Exercise> Update(Exercise entity)
        {
            try
            {
                var existingExercise = await GetById(entity.ExerciseId);
                if (existingExercise != null)
                {
                    existingExercise.Name = entity.Name;
                    existingExercise.DurationInMinutes = entity.DurationInMinutes;
                    existingExercise.Type = entity.Type;
                    await _lifeStyleContext.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException("Exercise not found");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the exercise.", ex);
            }
        }

        public async Task<Exercise?> GetById(int id)
        {
            try
            {
                var exercise = await _lifeStyleContext.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == id);
                return exercise;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the exercise by ID.", ex);
            }
        }

        public async Task<Exercise> GetByName(string name)
        {
            try
            {
                var exercise = await _lifeStyleContext.Exercises.FirstOrDefaultAsync(e => e.Name == name);
                return exercise;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the exercise by name.", ex);
            }
        }

        public int GetLastId()
        {
            try
            {
                return _lifeStyleContext.Exercises.Any() ? _lifeStyleContext.Exercises.Max(m => m.ExerciseId) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the last exercise ID.", ex);
            }
        }
    }
}