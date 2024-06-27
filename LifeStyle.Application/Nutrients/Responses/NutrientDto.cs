using LifeStyle.Domain.Models.Meal;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Application.Responses
{
    public class Nutrient
    {
        public int NutrientId { get; set; }

        [Required(ErrorMessage = "Calories is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Calories must be a positive number")]
        public double Calories { get; set; }

        [Required(ErrorMessage = "Protein is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Protein must be a positive number")]
        public double Protein { get; set; }
        [Required(ErrorMessage = "Carbohydrates is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Carbohydrates must be a positive number")]
        public double Carbohydrates { get; set; }

        [Required(ErrorMessage = "Fat is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fat must be a positive number")]
        public double Fat { get; set; }

        public static Nutrient FromNutrient(Nutrients nutrients) 
        {
            if (nutrients == null)
                return null;

            return new Nutrient
            {
              //  NutrientId = nutrients.NutrientId,
                Calories = nutrients.Calories,
                Protein = nutrients.Protein,
                Carbohydrates = nutrients.Carbohydrates,
                Fat = nutrients.Fat
            };
        }
       
    }
}
