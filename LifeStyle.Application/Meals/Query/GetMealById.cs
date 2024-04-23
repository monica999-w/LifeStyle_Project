using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;

using MediatR;


namespace LifeStyle.Application.Meals.Query
{
    public record GetMealById(int MealId) : IRequest<MealDto>;

    public class GetMealByIdHandler : IRequestHandler<GetMealById, MealDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMealByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MealDto> Handle(GetMealById request, CancellationToken cancellationToken)
        {
            var meal = await _unitOfWork.MealRepository.GetById(request.MealId);
            if (meal == null)
                throw new Exception($"Meal with ID {request.MealId} not found");

            return MealDto.FromMeal(meal);
        }
    }

}
