using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Application.Responses
{
    public class MealDto
    {
       // public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? Name { get; set; }
        [EnumDataType(typeof(MealType))]
        public MealType MealType { get; set; }
        public NutrientDto? Nutrients { get; set; }

        public static MealDto FromMeal(Meal meal)
        {
           
            var nutrientsDto = meal.Nutrients != null ? NutrientDto.FromNutrient(meal.Nutrients) : null;

            return new MealDto
            {
                //Id = meal.MealId,
                Name = meal.Name,
                MealType = meal.MealType,
                Nutrients = nutrientsDto
            };
        }
    }
}
