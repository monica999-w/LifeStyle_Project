using LifeStyle.Domain.Models.Meal;

namespace LifeStyle.Application.Responses
{
    public class NutrientDto
    {
        public int NutrientId { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }

        public static NutrientDto FromNutrient(Nutrients nutrients) 
        {
            if (nutrients == null)
                return null;

            return new NutrientDto
            {
                NutrientId = nutrients.NutrientId,
                Calories = nutrients.Calories,
                Protein = nutrients.Protein,
                Carbohydrates = nutrients.Carbohydrates,
                Fat = nutrients.Fat
            };
        }
        public static Nutrients FromNutrientDto(NutrientDto nutrientDto)
        {

            return new Nutrients
                (
                 nutrientDto.NutrientId,
                 nutrientDto.Calories,
                 nutrientDto.Protein,
                 nutrientDto.Carbohydrates,
                 nutrientDto.Fat
                 );
        }
    }
}
