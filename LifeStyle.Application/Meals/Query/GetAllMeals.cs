using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;

using MediatR;

namespace LifeStyle.Application.Meals.Query
{
    public record GetAllMeals : IRequest<List<MealDto>>;


    public class GetAllMealsHandler : IRequestHandler<GetAllMeals, List<MealDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMealsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MealDto>> Handle(GetAllMeals request, CancellationToken cancellationToken)
        {
            var meals = await _unitOfWork.MealRepository.GetAll(); 
            return meals.Select(MealDto.FromMeal).ToList();
        }
    }
}
