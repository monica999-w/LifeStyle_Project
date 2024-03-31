using LifeStyle.Enums;

namespace LifeStyle.Models.Meal
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public Nutrients Nutrients { get; set; }

        public Meal(int id,string name, MealType mealType, Nutrients nutrients)
        {
            Id = id;
            Name = name;
            MealType = mealType;
            Nutrients = nutrients;
        }
    }
}
