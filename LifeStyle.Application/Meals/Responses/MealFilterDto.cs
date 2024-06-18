using LifeStyle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Meals.Responses
{
    public class MealFilterDto
    {
        [EnumDataType(typeof(MealType))]
        public MealType? MealType { get; set; }
        public List<AllergyType> Allergies { get; set; } = new List<AllergyType>();

        public List<DietType> Diets { get; set; } = new List<DietType>();
        public int? MaxCalories { get; set; }

    }
}
