using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using System.ComponentModel.DataAnnotations;



namespace LifeStyle.Application.Responses
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? Name { get; set; }
        public int DurationInMinutes { get; set; }
        [EnumDataType(typeof(ExerciseType))]
        public  ExerciseType Type { get; set; }

        public static ExerciseDto FromExercise(Exercise exercise)
        {
            return new ExerciseDto
            {
                Id = exercise.ExerciseId,
                Name = exercise.Name,
                DurationInMinutes = exercise.DurationInMinutes,
                Type = exercise.Type
            };
        }

       
    }
}
