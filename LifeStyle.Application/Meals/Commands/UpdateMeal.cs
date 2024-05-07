using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;


namespace LifeStyle.Application.Commands
{
    public record UpdateMeal(int MealId, string Name, MealType MealType, Nutrients Nutrients) : IRequest<MealDto>;

    public class UpdateMealHandler : IRequestHandler<UpdateMeal, MealDto>
    {

        private readonly IUnitOfWork _unitOfWork;

        public UpdateMealHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MealDto> Handle(UpdateMeal request, CancellationToken cancellationToken)
        {
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

                return MealDto.FromMeal(meal);
            }
            catch (NotFoundException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new NotFoundException("Failed to update meal: " + ex.Message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataValidationException("Failed to update meal", ex);
            }
        }
    }
}