using LifeStyle.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Domain.Models.Meal
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? Name { get; set; }

        [EnumDataType(typeof(MealType))]
        public MealType MealType { get; set; }
        public Nutrients? Nutrients { get; set; } 
       
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
