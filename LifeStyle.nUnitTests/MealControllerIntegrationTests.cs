using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using LifeStyle.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.IntegrationTests
{
    public class MealControllerIntegrationTests
    {

        [Fact]
        public async Task MealController_GetAll_ReturnsOkWithCorrectData()
        {// Arrange
            var numberOfUser = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedMeals(numberOfUser);
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            Assert.NotNull(mediator);
            Assert.NotNull(mapper);

            var controller = new MealController(mediator, mapper);

            // Act
            var requestResult = await controller.GetAllMeals();

            // Assert
            var result = requestResult.Result as OkObjectResult;
            var meal = result!.Value as List<MealDto>;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            Assert.NotNull(meal);
            Assert.Equal(numberOfUser, meal.Count);
        }


        [Fact]
        public async Task MealsController_CreateMeal_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, null, nutrientRepository, mealRepository, null, null);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new MealController(mediator, mapper);

            var nutrientDto = new NutrientDto { Calories = 500, Protein = 20, Carbohydrates = 50, Fat = 10 };
            var mealDto = new MealDto { Name = "Test Meal", MealType = MealType.Breakfast, Nutrients = new Nutrients() };

            // Act
            var requestResult = await controller.CreateMeal(mealDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var createdMeal = result.Value as MealDto;
            Assert.NotNull(createdMeal);
            Assert.Equal("Test Meal", createdMeal.Name);
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

            var controller = new MealController(mediator, mapper);

            // Seed the database with a meal to delete
            var nutrient = new Nutrients { Calories = 500, Protein = 20, Carbohydrates = 50, Fat = 10 };
            var meal = new Meal { Name = "Test Meal", MealType = MealType.Breakfast, Nutrients = nutrient };
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

            var controller = new MealController(mediator, mapper);

            // Act
            var invalidMealId = 999;
            var requestResult = await controller.DeleteMeal(invalidMealId);

            // Assert
            var result = requestResult as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Meal not found: Failed to delete meal: Meal with ID 999 not found", result.Value);
        }

        [Fact]
        public async Task MealsController_UpdateMeal_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, null, nutrientRepository, mealRepository, null, null);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new MealController(mediator, mapper);

            // Seed the database with a meal to update
            var nutrient = new Nutrients{ Calories = 500, Protein = 20, Carbohydrates = 50, Fat = 10 };
            var meal = new Meal { Name = "Old Meal", MealType = MealType.Breakfast, Nutrients = nutrient };
            await dbContext.Meals.AddAsync(meal);
            await dbContext.SaveChangesAsync();

            // Act
            var updatedMealDto = new MealDto { Id = meal.MealId, Name = "Updated Meal", MealType = MealType.Lunch, Nutrients = new Nutrients { Calories = 600, Protein = 30, Carbohydrates = 60, Fat = 20 } };
            var requestResult = await controller.UpdateMeal(meal.MealId, updatedMealDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var resultValue = result.Value as MealDto;
            Assert.NotNull(resultValue);
            Assert.Equal("Updated Meal", resultValue.Name);
            Assert.Equal(MealType.Lunch, resultValue.MealType);
            Assert.Equal(600, resultValue.Nutrients.Calories);
            Assert.Equal(30, resultValue.Nutrients.Protein);
            Assert.Equal(60, resultValue.Nutrients.Carbohydrates);
            Assert.Equal(20, resultValue.Nutrients.Fat);
        }
    }
}
