using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record UpdateMeal(int MealId, string Name, MealType MealType, Nutrients Nutrients) : IRequest<Meal>;

    public class UpdateMealHandler : IRequestHandler<UpdateMeal, Meal>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<UpdateNutrient, Nutrients> _updateNutrientHandler;

        public UpdateMealHandler(IUnitOfWork unitOfWork, IRequestHandler<UpdateNutrient, Nutrients> updateNutrientHandler)
        {
            _unitOfWork = unitOfWork;
            _updateNutrientHandler = updateNutrientHandler;
            Log.Information("UpdateMealHandler instance created.");
        }

        public async Task<Meal> Handle(UpdateMeal request, CancellationToken cancellationToken)
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

                if (request.Nutrients == null)
                {
                    throw new ArgumentNullException("Nutrients cannot be null");
                }
                // Update Nutrients
                var updatedNutrient = await _updateNutrientHandler.Handle(new UpdateNutrient(request.MealId, request.Nutrients.Calories, request.Nutrients.Protein, request.Nutrients.Carbohydrates, request.Nutrients.Fat), cancellationToken);

                meal.Nutrients = updatedNutrient;

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.MealRepository.Update(meal);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                return meal;
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