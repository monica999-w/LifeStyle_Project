using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Exercises.Responses;
using LifeStyle.Application.Meals.Responses;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Paged;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace LifeStyle.Application.Meals.Query
{

    public record FilterMealQuery(MealFilterDto Filter, int PageNumber, int PageSize) : IRequest<PagedResult<Meal>>;

    public class FilterMealQueryHandler : IRequestHandler<FilterMealQuery, PagedResult<Meal>>
    {
        private readonly IRepository<Meal> _mealRepository;
        private readonly IMapper _mapper;

        public FilterMealQueryHandler(IRepository<Meal> mealRepository,IMapper mapper)
        {
            _mealRepository = mealRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<Meal>> Handle(FilterMealQuery request, CancellationToken cancellationToken)
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

            var totalCount = meals.Count();

            var items = meals.Skip((request.PageNumber - 1) * request.PageSize)
                                 .Take(request.PageSize)
                                 .ToList();

            var mealDtos = _mapper.Map<List<Meal>>(items);
            return new PagedResult<Meal>(mealDtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

