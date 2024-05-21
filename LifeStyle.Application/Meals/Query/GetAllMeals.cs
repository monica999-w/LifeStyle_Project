using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;

namespace LifeStyle.Application.Meals.Query
{
    public record GetAllMeals : IRequest<List<Meal>>;

    public class GetAllMealsHandler : IRequestHandler<GetAllMeals, List<Meal>>
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public GetAllMealsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Log.Information("GetAllMealsHandler instance created.");
        }

        public async Task<List<Meal>> Handle(GetAllMeals request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllMeals command...");
            try
            {
                var meals = await _unitOfWork.MealRepository.GetAll();
                return meals;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all meals");
                throw new Exception("Failed to retrieve all meals", ex);
            }
        }
    }
}
