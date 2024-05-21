using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Application.Responses
{
    public class MealDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? Name { get; set; }
        [EnumDataType(typeof(MealType))]
        public MealType MealType { get; set; }
        public Nutrients Nutrients { get; set; }

    }
}
