using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;


namespace LifeStyle.Application.Responses
{
    public class MealDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public required Nutrients Nutrients { get; set; }

        public static MealDto FromMeal(Meal meal)
        {
            return new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                MealType = meal.MealType,
                Nutrients = meal.Nutrients
            };
        }

        public static Meal FromMealDto(MealDto mealDto)
        {
            return new Meal
                (
                 mealDto.Id,
                 mealDto.Name,
                 mealDto.MealType,
                 mealDto.Nutrients
                 );
        }
    }
}
