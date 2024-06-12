using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.ComponentModel.DataAnnotations;


namespace LifeStyle.Application.Commands
{
    public record CreateMeal(string MealName, MealType MealType, Nutrients Nutrients, List<string> Ingredients, string PreparationInstructions, int EstimatedPreparationTimeInMinutes, List<AllergyType> Allergies, List<DietType> Diets, IFormFile Image) : IRequest<Meal>;

    public class CreateMealHandler : IRequestHandler<CreateMeal, Meal>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestHandler<CreateNutrient, Nutrients> _createNutrientHandler;
        private readonly IFileService _fileService;

        public CreateMealHandler(IUnitOfWork unitOfWork, IRequestHandler<CreateNutrient, Nutrients> createNutrientHandler, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _createNutrientHandler = createNutrientHandler;
            _fileService = fileService;
            Log.Information("CreateMealHandler instance created.");
        }

        public async Task<Meal> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Handling CreateMeal command...");

                var nutrient = await _createNutrientHandler.Handle(
                    new CreateNutrient(request.Nutrients.Calories, request.Nutrients.Protein, request.Nutrients.Carbohydrates, request.Nutrients.Fat),
                    cancellationToken);

                Log.Information("New nutrient created: {NutrientId}", nutrient.NutrientId);

                

                var existingMeal = await _unitOfWork.MealRepository.GetByName(request.MealName);
                if (existingMeal != null)
                {
                    Log.Error($"A meal with the name '{request.MealName}' already exists");
                    throw new AlreadyExistsException($"A meal with the name '{request.MealName}' already exists");
                }

                string imageUrl = string.Empty;
                if (request.Image != null && request.Image.Length > 0)
                {
                    imageUrl = await _fileService.SaveFileAsync(request.Image,"meal_images"); 
                }

                await _unitOfWork.BeginTransactionAsync();

                var newMeal = new Meal
                {
                    MealName = request.MealName,
                    MealType = request.MealType,
                    Nutrients = nutrient,
                    Ingredients = request.Ingredients,
                    PreparationInstructions = request.PreparationInstructions,
                    EstimatedPreparationTimeInMinutes = request.EstimatedPreparationTimeInMinutes,
                    Allergies = request.Allergies,
                    Diets = request.Diets,
                    Image = imageUrl
                };

                await _unitOfWork.MealRepository.Add(newMeal);
                await _unitOfWork.SaveAsync();
                
                await _unitOfWork.CommitTransactionAsync();
                Log.Information("Meal created successfully: {MealId}", newMeal.MealId);

                return newMeal;
            }
            catch (AlreadyExistsException ex)
            {
                Log.Error(ex, "Already exists error");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create meal");
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Failed to create meal", ex);
            }
        }
    }
}

