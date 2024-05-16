using LifeStyle.Domain.Enums;

using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace LifeStyle.Domain.Models.Exercises
{
    public class Exercise   
    {
        [Key]
        public int ExerciseId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? Name { get; set; } 
        public int DurationInMinutes { get; set; }

        [EnumDataType(typeof(ExerciseType))]
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
