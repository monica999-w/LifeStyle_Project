using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Paged;
using MediatR;
using Serilog;

namespace LifeStyle.Application.Meals.Query
{
    public record GetAllMeals(int PageNumber, int PageSize) : IRequest<PagedResult<Meal>>;


    public class GetAllMealsHandler : IRequestHandler<GetAllMeals, PagedResult<Meal>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllMealsHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("GetAllMealsHandler instance created.");
        }

        public async Task<PagedResult<Meal>> Handle(GetAllMeals request, CancellationToken cancellationToken)
        {
            Log.Information("Handling GetAllMeals command...");

            try
            {
                var meals = await _unitOfWork.MealRepository.GetAll();
                var totalCount = meals.Count();
                var items = meals.Skip((request.PageNumber - 1) * request.PageSize)
                                 .Take(request.PageSize)
                                 .ToList();
                var mealDtos = _mapper.Map<List<Meal>>(items);
                return new PagedResult<Meal>(mealDtos, totalCount, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve all meals");
                throw new Exception("Failed to retrieve all meals", ex);
            }
        }
    }
}