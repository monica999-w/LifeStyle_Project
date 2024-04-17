using LifeStyle.Domain.Enums;
using LifeStyle.Models.Planner;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Domain.Models.Meal
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public MealType MealType { get; set; }
        public Nutrients Nutrients { get; set; } = new Nutrients();
       
         public Meal()
        {
        }

        public Meal(int mealId, string name, MealType mealType, Nutrients nutrients)
        {
            MealId = mealId;
            Name = name;
            MealType = mealType;
            Nutrients = nutrients;
        }

    }
}
