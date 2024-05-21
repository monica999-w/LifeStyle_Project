using LifeStyle.Application.Abstractions;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record DeleteMeal(int MealId) : IRequest<Unit>;

    public class DeleteMealHandler : IRequestHandler<DeleteMeal, Unit>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<DeleteNutrient, Nutrients> _deleteNutrientHandler;

        public DeleteMealHandler(IUnitOfWork unitOfWork, IRequestHandler<DeleteNutrient, Nutrients> deleteNutrientHandler)
        {
            _unitOfWork = unitOfWork;
            _deleteNutrientHandler = deleteNutrientHandler;
            Log.Information("DeleteMealHandler instance created.");
        }

        public async Task<Unit> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {
            Log.Information("Handling DeleteMeal command for Meal ID {MealId}...", request.MealId);

            try
            {
                var meal = await _unitOfWork.MealRepository.GetById(request.MealId);

                if (meal == null)
                {
                    throw new NotFoundException($"Meal with ID {request.MealId} not found");
                }

              
                var nutrient = await _unitOfWork.NutrientRepository.GetById(meal.Nutrients.NutrientId);
                if (nutrient != null)
                {
                    await _deleteNutrientHandler.Handle(new DeleteNutrient(nutrient.NutrientId), cancellationToken);
                }

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.MealRepository.Remove(meal);

                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
                Log.Error(ex, "Meal not found");
                throw new NotFoundException("Failed to delete meal: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete meal");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to delete meal", ex);
            }
        }
    }
}
