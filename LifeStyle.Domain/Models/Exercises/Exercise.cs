using LifeStyle.Domain.Enums;
using LifeStyle.Models.Planner;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Domain.Models.Exercises
{
    public class Exercise   
    {
        [Key]
        public int ExerciseId { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; } 
        public int DurationInMinutes { get; set; }
        public ExerciseType Type { get; set; }
      

        public Exercise()
        {
        }


        public Exercise(int exerciseId, string name, int durationInMinutes, ExerciseType type)
        {
            this.ExerciseId = exerciseId;
            Name = name;
            DurationInMinutes = durationInMinutes;
            Type = type;

        }

       
    }
}
