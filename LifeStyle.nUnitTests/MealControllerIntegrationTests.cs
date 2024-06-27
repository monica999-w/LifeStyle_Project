using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Services;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Paged;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using LifeStyle.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeStyle.IntegrationTests
{
    public class MealControllerIntegrationTests
    {
        [Fact]
        public async Task MealController_GetAll_ReturnsOkWithCorrectData()
        {
            // Arrange
            var numberOfMeals = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedMeals(numberOfMeals);
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);
            var mockFileService = new Mock<IFileService>();

            Assert.NotNull(mediator);
            Assert.NotNull(mapper);

            var controller = new MealController(mediator, mapper, mockFileService.Object);

            // Act
            var requestResult = await controller.GetAllMeals();

            // Assert
            var result = requestResult.Result as OkObjectResult;
            Assert.NotNull(result);

            var meals = result.Value as PagedResult<Meal>;
            Assert.NotNull(meals);

        }


        [Fact]
        public async Task MealsController_CreateMeal_WithValidData_ReturnOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, null, nutrientRepository, mealRepository, null, null);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);
            var mockFileService = new Mock<IFileService>();

            var controller = new MealController(mediator, mapper, mockFileService.Object);

            var nutrientDto = new Nutrient { Calories = 500, Protein = 20, Carbohydrates = 50, Fat = 10 };
            var mealDto = new MealDto { MealName = "Test Meal", MealType = MealType.Breakfast, Nutrients = new Nutrients() };

            // Act
            var requestResult = await controller.CreateMeal(mealDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var createdMeal = result.Value as MealDto;
            Assert.NotNull(createdMeal);
            Assert.Equal("Test Meal", createdMeal.MealName);
        }

        [Fact]
        public async Task MealsController_DeleteMeal_WithValidId_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, null, nutrientRepository, mealRepository, null, null);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);
            var mockFileService = new Mock<IFileService>();

            var controller = new MealController(mediator, mapper, mockFileService.Object);

            // Seed the database with a meal to delete
            var nutrient = new Nutrients { Calories = 500, Protein = 20, Carbohydrates = 50, Fat = 10 };
            var meal = new Meal { MealName = "Test Meal", MealType = MealType.Breakfast, Nutrients = nutrient };
            await dbContext.Meals.AddAsync(meal);
            await dbContext.SaveChangesAsync();

            // Act
            var requestResult = await controller.DeleteMeal(meal.MealId);

            // Assert
            var result = requestResult as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task MealsController_DeleteMeal_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, null, nutrientRepository, mealRepository, null, null);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);
            var mockFileService = new Mock<IFileService>();

            var controller = new MealController(mediator, mapper, mockFileService.Object);

            // Act
            var invalidMealId = 999;
            var requestResult = await controller.DeleteMeal(invalidMealId);

            // Assert
            var result = requestResult as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Meal not found: Failed to delete meal: Meal with ID 999 not found", result.Value);
        }
    }
}
