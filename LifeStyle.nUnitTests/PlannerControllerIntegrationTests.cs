using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using LifeStyle.IntegrationTests.Helpers;
using LifeStyle.Models.Planner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.IntegrationTests
{
    public class PlannerControllerIntegrationTests
    {

        [Fact]
        public async Task PlannersController_CreatePlanner_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var userProfileRepository = new UserRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, null, mealRepository, userProfileRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new PlannerController(mediator, mapper);

          
            var user = new UserProfile { Email = "user@example.com", PhoneNumber = "1234567890", Height = 180, Weight = 75 };
            var meal1 = new Meal { Name = "Meal 1", MealType = MealType.Breakfast };
            var meal2 = new Meal { Name = "Meal 2", MealType = MealType.Lunch };
            var exercise1 = new Exercise { Name = "Exercise 1", DurationInMinutes= 10,Type=ExerciseType.Swimming };
            var exercise2 = new Exercise { Name = "Exercise 2", DurationInMinutes = 10, Type = ExerciseType.Swimming };

            await dbContext.UserProfiles.AddAsync(user);
            await dbContext.Meals.AddAsync(meal1);
            await dbContext.Meals.AddAsync(meal2);
            await dbContext.Exercises.AddAsync(exercise1);
            await dbContext.Exercises.AddAsync(exercise2);
            await dbContext.SaveChangesAsync();

            // Act
            var plannerDto = new PlannerDto
            {
                ProfileId = user.ProfileId,
                MealIds = new List<int> { meal1.MealId, meal2.MealId },
                ExerciseIds = new List<int> { exercise1.ExerciseId, exercise2.ExerciseId }
            };

            var requestResult = await controller.CreatePlanner(plannerDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var resultValue = result.Value as PlannerDto;
            Assert.NotNull(resultValue);
            Assert.Equal(plannerDto.ProfileId, resultValue.ProfileId);
            Assert.Equal(plannerDto.MealIds.Count, resultValue.MealIds.Count);
            Assert.Equal(plannerDto.ExerciseIds.Count, resultValue.ExerciseIds.Count);
        }

        [Fact]
        public async Task PlannersController_DeletePlanner_WithValidPlannerId_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var userProfileRepository = new UserRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, null, mealRepository, userProfileRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new PlannerController(mediator, mapper);

            // Seed the database with necessary data
            var user = new UserProfile { Email = "user@example.com", PhoneNumber = "1234567890", Height = 180, Weight = 75 };
            await dbContext.UserProfiles.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var planner = new Planner(user);
            await dbContext.Planners.AddAsync(planner);
            await dbContext.SaveChangesAsync();

            // Act
            var requestResult = await controller.DeletePlanner(planner.PlannerId);

            // Assert
            var result = requestResult as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task PlannersController_DeletePlanner_WithInvalidPlannerId_ReturnsNotFound()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var userProfileRepository = new UserRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, null, mealRepository, userProfileRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new PlannerController(mediator, mapper);

            // Act
            var invalidPlannerId = 999; // Assuming this ID does not exist
            var requestResult = await controller.DeletePlanner(invalidPlannerId);

            // Assert
            var result = requestResult as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"Planner with ID {invalidPlannerId} not found", result.Value);
        }
    }
}
