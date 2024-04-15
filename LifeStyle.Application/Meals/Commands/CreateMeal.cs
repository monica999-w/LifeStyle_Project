using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record CreateMeal(string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class CreateMealHandler : IRequestHandler<CreateMeal, MealDto>
    {
       
        private readonly IRepository<Meal> _mealRepository;


        public CreateMealHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<MealDto> Handle(CreateMeal request, CancellationToken cancellationToken)
        {

            var meal = new Meal
            (
               id: GetNextId(),
               name: request.Name,
               mealType: request.MealType,
               nutrients: request.Nutrients
            );

            await _mealRepository.Add(meal);
            return MealDto.FromMeal(meal);
        }
        private int GetNextId()
        {
            var lastId = _mealRepository.GetLastId();
            return lastId + 1; 
        }
    }
}
