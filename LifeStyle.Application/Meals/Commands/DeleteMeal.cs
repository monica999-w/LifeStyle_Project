using LifeStyle.Aplication.Interfaces;

using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record DeleteMeal(int MealId) : IRequest<Unit>;

    public class DeleteMealHandler : IRequestHandler<DeleteMeal, Unit>
    {
     
        private readonly IRepository<Meal> _mealRepository;


        public DeleteMealHandler(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<Unit> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {
            var meal = await _mealRepository.GetById(request.MealId);
            if (meal == null)
            {
                throw new KeyNotFoundException("Exercise not found");
            }

            await _mealRepository.Remove(meal);
            return Unit.Value;
        }
    }
}
