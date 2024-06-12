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
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 50, ErrorMessage = "Description length must be between 50 and 500 characters")]
        public string Description { get; set; }
        public string VideoLink { get; set; }
        [EnumDataType(typeof(ExerciseType))]
        public  ExerciseType Type { get; set; }
       [EnumDataType(typeof(Equipment))]
         public Equipment Equipment { get; set; }
        [EnumDataType(typeof(MajorMuscle))]
        public MajorMuscle MajorMuscle { get; set; }

    }
}
