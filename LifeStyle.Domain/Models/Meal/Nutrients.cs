using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Meal
{
    public class Nutrients
    {
        [Key]
        public  int NutrientId { get; set; }

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


        public Nutrients()
        {
        }

        public Nutrients(int nutrientId, double calories, double protein, double carbohydrates, double fat)
        {
            NutrientId = nutrientId;
            Calories = calories;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fat = fat;
        }

    }
}

