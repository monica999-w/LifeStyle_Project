using LifeStyle.Enums;
using LifeStyle.Interfaces;
using LifeStyle.Models.User;

namespace LifeStyle.Models.Exercises
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationInMinutes { get; set; }
        public ExerciseType Type { get; set; }

        public Exercise(int id ,string name, int durationInMinutes, ExerciseType type)
        {
            Id = id;
            Name = name;
            DurationInMinutes = durationInMinutes;
            Type = type;
           
        }
    }
}
