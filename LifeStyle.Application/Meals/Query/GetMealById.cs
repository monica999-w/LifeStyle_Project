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
    public record GetMealById(int MealId) : IRequest<MealDto>;

    public class GetMealByIdHandler : IRequestHandler<GetMealById, MealDto>
    {
        private readonly IRepository<Meal> _mealRepository;

        public GetMealByIdHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<MealDto> Handle(GetMealById request, CancellationToken cancellationToken)
        {
            var meal = await _mealRepository.GetById(request.MealId);
            if (meal == null)
                throw new Exception($"Meal with ID {request.MealId} not found");

            return MealDto.FromMeal(meal);
        }
    }

}
