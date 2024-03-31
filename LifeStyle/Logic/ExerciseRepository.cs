using LifeStyle.Enums;
using LifeStyle.Interfaces;
using LifeStyle.Models.Exercises;
using LifeStyle.Models.Meal;
using LifeStyle.Models.User;

namespace LifeStyle.Logic
{

    public class ExerciseRepository : IRepository<Exercise>
    {

        private readonly List<Exercise> _exercises = new List<Exercise>();

        public ExerciseRepository()
        {
            
            _exercises.Add(new Exercise(1, "Running", 30, ExerciseType.Cardio));
            _exercises.Add(new Exercise(2, "Weightlifting", 45, ExerciseType.Yoga));
            _exercises.Add(new Exercise(3, "Yoga", 60, ExerciseType.Cardio));

        }

        public async Task<IEnumerable<Exercise>> GetAll()
        {
            await Task.Delay(0);
            return _exercises;
        }

        public async Task Add(Exercise entity)
        {
            await Task.Delay(0);
            _exercises.Add(entity);
        }

        public async Task Remove(Exercise entity)
        {
            await Task.Delay(0);
            var existingExercise = await GetById(entity.Id);
            if (existingExercise != null)
            {
                _exercises.Remove(existingExercise);
            }
            else
            {
                throw new KeyNotFoundException("Meal not found");
            }
        }

        public async Task Update(Exercise entity)
        {
            var existingExercise = await GetById(entity.Id);
            if (existingExercise != null)
            {
               
                existingExercise.Name = entity.Name;
                existingExercise.DurationInMinutes = entity.DurationInMinutes;
                existingExercise.Type = entity.Type;
            }
            else
            {
                throw new KeyNotFoundException("Exercise not found");
            }
        }

        public async Task<Exercise?> GetById(int id)
        {
            await Task.Delay(0);
            var exercise = _exercises.FirstOrDefault(e => e.Id == id);
            if (exercise == null)
            {
                throw new KeyNotFoundException("Exercise not found");
            }
            return exercise;
        }
    }
}