using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;


namespace LifeStyle.Application.Responses
{
    public class MealDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public MealType MealType { get; set; }
        public NutrientDto? Nutrients { get; set; }

        public static MealDto FromMeal(Meal meal)
        {
           
            var nutrientsDto = meal.Nutrients != null ? NutrientDto.FromNutrient(meal.Nutrients) : null;

            return new MealDto
            {
                Id = meal.MealId,
                Name = meal.Name,
                MealType = meal.MealType,
                Nutrients = nutrientsDto
            };
        }
    }
}
