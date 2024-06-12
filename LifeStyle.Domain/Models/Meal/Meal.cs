using LifeStyle.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Domain.Models.Meal
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
        public string? MealName { get; set; }

        [EnumDataType(typeof(MealType))]
        public MealType MealType { get; set; }

        public Nutrients? Nutrients { get; set; }

        public List<AllergyType> Allergies { get; set; } = new List<AllergyType>();

        public List<DietType> Diets { get; set; } = new List<DietType>();

        public List<string> Ingredients { get; set; } = new List<string>();

        [Required(ErrorMessage = "Preparation instructions are required")]
        public string PreparationInstructions { get; set; }

        [Required(ErrorMessage = "Estimated preparation time is required")]
        public int EstimatedPreparationTimeInMinutes { get; set; }
        public string Image { get; set; }


        public Meal()
        {
        }

        public Meal(int mealId, string name, MealType mealType, Nutrients nutrients)
        {
            MealId = mealId;
            MealName = name;
            MealType = mealType;
            Nutrients = nutrients;
        }

    }
}
