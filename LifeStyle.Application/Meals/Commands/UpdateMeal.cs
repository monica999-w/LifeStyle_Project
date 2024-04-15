using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record UpdateMeal(int MealId, string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class UpdateMealHandler : IRequestHandler<UpdateMeal, MealDto>
    {
     
        private readonly IRepository<Meal> _mealRepository;

        public UpdateMealHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<MealDto> Handle(UpdateMeal request, CancellationToken cancellationToken)
        {
            var meal = await _mealRepository.GetById(request.MealId);
            if (meal == null)
            {
                throw new KeyNotFoundException($"Meal with ID {request.MealId} not found");
            }

            meal.Name = request.Name;
            meal.MealType = request.MealType;
            meal.Nutrients = request.Nutrients;

            await _mealRepository.Update(meal);
            return MealDto.FromMeal(meal);
        }
    }
}
