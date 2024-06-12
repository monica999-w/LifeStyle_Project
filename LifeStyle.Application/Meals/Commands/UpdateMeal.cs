using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;


namespace LifeStyle.Application.Commands
{
    public record UpdateMeal(
    int MealId,
    string Name,
    MealType MealType,
    Nutrients Nutrients,
    List<string> Ingredients,
    string PreparationInstructions,
    int EstimatedPreparationTimeInMinutes,
    IFormFile Image,
    List<AllergyType> Allergies,
    List<DietType> Diets) : IRequest<Meal>;


    public class UpdateMealHandler : IRequestHandler<UpdateMeal, Meal>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<UpdateNutrient, Nutrients> _updateNutrientHandler;
        private readonly IFileService _fileService;

        public UpdateMealHandler(IUnitOfWork unitOfWork, IRequestHandler<UpdateNutrient, Nutrients> updateNutrientHandler, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _updateNutrientHandler = updateNutrientHandler;
            _fileService = fileService;
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

                meal.MealName = request.Name;
                meal.MealType = request.MealType;
                meal.PreparationInstructions = request.PreparationInstructions;
                meal.EstimatedPreparationTimeInMinutes = request.EstimatedPreparationTimeInMinutes;
                meal.Allergies = request.Allergies;
                meal.Diets = request.Diets;
                meal.Ingredients = request.Ingredients;

                if (request.Nutrients == null)
                {
                    throw new ArgumentNullException("Nutrients cannot be null");
                }

                // Update Nutrients
                var updatedNutrient = await _updateNutrientHandler.Handle(
                    new UpdateNutrient(meal.Nutrients.NutrientId, request.Nutrients.Calories, request.Nutrients.Protein, request.Nutrients.Carbohydrates, request.Nutrients.Fat),
                    cancellationToken);

                meal.Nutrients = updatedNutrient;
                if (request.Image != null && request.Image.Length > 0)
                {
                    var imageUrl = await _fileService.SaveFileAsync(request.Image, "meal_images");
                    meal.Image = imageUrl;
                }

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