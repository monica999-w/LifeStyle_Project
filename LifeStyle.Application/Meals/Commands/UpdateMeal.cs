using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record UpdateMeal(int MealId, string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class UpdateMealHandler : IRequestHandler<UpdateMeal, MealDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public UpdateMealHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            Log.Information("UpdateMealHandler instance created.");
        }

        public async Task<MealDto> Handle(UpdateMeal request, CancellationToken cancellationToken)
        {
            Log.Information("Handling UpdateMeal command for Meal ID {MealId}...", request.MealId);

            try
            {
                var meal = await _unitOfWork.MealRepository.GetById(request.MealId);
                if (meal == null)
                {
                    throw new NotFoundException($"Meal with ID {request.MealId} not found");
                }

                meal.Name = request.Name;
                meal.MealType = request.MealType;
                meal.Nutrients = request.Nutrients;

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.MealRepository.Update(meal);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MealDto>(meal);
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Meal not found");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update meal");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to update meal", ex);
            }
        }
    }
}