
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Application.Responses
{
    public class MealDto
    {
        public int MealId { get; set; }
        public string? MealName { get; set; }
        public MealType MealType { get; set; }
        public Nutrients? Nutrients { get; set; }
        public List<AllergyType> Allergies { get; set; } = new List<AllergyType>();
        public List<DietType> Diets { get; set; } = new List<DietType>();
        public List<string> Ingredients { get; set; } = new List<string>();
        public string? PreparationInstructions { get; set; }
        public int EstimatedPreparationTimeInMinutes { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
