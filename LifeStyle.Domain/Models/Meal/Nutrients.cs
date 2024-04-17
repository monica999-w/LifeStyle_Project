using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Meal
{
    public class Nutrients
    {
        [Key]
        public  int NutrientId { get; set; } 
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
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

