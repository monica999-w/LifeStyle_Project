using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Application.Meals.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Meals.Query
{

    public record FilterMealQuery(MealFilterDto Filter) : IRequest<IEnumerable<Meal>>;

    public class FilterMealQueryHandler : IRequestHandler<FilterMealQuery, IEnumerable<Meal>>
    {
        private readonly IRepository<Meal> _mealRepository;

        public FilterMealQueryHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<IEnumerable<Meal>> Handle(FilterMealQuery request, CancellationToken cancellationToken)
        {
            var meals = await _mealRepository.GetAll();

            if (request.Filter.MealType.HasValue)
            {
                meals = meals.Where(m => m.MealType == request.Filter.MealType.Value).ToList();
            }

            if (request.Filter.Diets != null && request.Filter.Diets.Any())
            {
                meals = meals.Where(m => m.Diets.Any(d => request.Filter.Diets.Contains(d))).ToList();
            }

            if (request.Filter.Allergies != null && request.Filter.Allergies.Any())
            {
                meals = meals.Where(m => m.Allergies.Any(a => request.Filter.Allergies.Contains(a))).ToList();
            }

            if (request.Filter.MaxCalories.HasValue)
            {
                meals = meals.Where(m => m.Nutrients.Calories <= request.Filter.MaxCalories.Value).ToList();
            }

            return meals;
        }
    }
}

