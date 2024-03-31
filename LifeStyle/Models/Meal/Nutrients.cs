namespace LifeStyle.Models.Meal
{
    public class Nutrients
    {
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }

        public Nutrients(double calories, double protein, double carbohydrates,double fat)
        {
            Calories = calories;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fat = fat;
        }

    }
}
   
