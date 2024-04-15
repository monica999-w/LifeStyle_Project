using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.Application.Meals.Query
{
    public record GetAllMeals : IRequest<List<MealDto>>;


    public class GetAllMealsHandler : IRequestHandler<GetAllMeals, List<MealDto>>
    {
        private readonly IRepository<Meal> _mealRepository;

        public GetAllMealsHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<List<MealDto>> Handle(GetAllMeals request, CancellationToken cancellationToken)
        {
            var meals = await _mealRepository.GetAll();
            return meals.Select(MealDto.FromMeal).ToList();
        }
    }
}
