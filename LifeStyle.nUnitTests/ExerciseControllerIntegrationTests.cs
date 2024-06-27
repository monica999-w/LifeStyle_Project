using AutoMapper;
using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Infrastructure.Context;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using LifeStyle.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;



namespace LifeStyle.IntegrationTests
{
    public class ExerciseControllerIntegrationTests
    {
        [Fact]
        public async Task ExerciseController_GetAllExercises_ReturnsOkWithCorrectData()
        {// Arrange
            var numberOfExercise = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedExercises(numberOfExercise);
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

            var controller = new ExerciseController(mediator, mapper);

            // Act
            var requestResult = await controller.GetAllExercises();

            // Assert
            var result = requestResult.Result as OkObjectResult;
            var categories = result!.Value as List<ExerciseDto>;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            Assert.NotNull(categories);
            Assert.Equal(numberOfExercise, categories.Count);
        }

        [Fact]
        public async Task ExerciseController_GetExerciseById_WithExistingId_ReturnsOkWithCorrectData()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new ExerciseController(mediator, mapper);

            // Add
            var sampleExercise = new Exercise { ExerciseId = 1, Name = "Sample Exercise", DurationInMinutes = 30, Type = ExerciseType.Cardio };
            await dbContext.Exercises.AddAsync(sampleExercise);
            await dbContext.SaveChangesAsync();

            // Act
            var requestResult = await controller.GetExerciseById(1); 

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);

            var exerciseDto = result!.Value as ExerciseDto;
            Assert.NotNull(exerciseDto);

            Assert.Equal("Sample Exercise", exerciseDto.Name);
            Assert.Equal(30, exerciseDto.DurationInMinutes);
            Assert.Equal(ExerciseType.Cardio, exerciseDto.Type);
        }

        [Fact]
        public async Task ExerciseController_Create_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new ExerciseController(mediator, mapper);

            // Act
            var newExerciseDto = new ExerciseDto
            {
                Name = "New Exercise",
                DurationInMinutes = 45,
                Type = ExerciseType.Aerobics
            };
            var requestResult = await controller.Create(newExerciseDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }



        [Fact]
        public async Task ExerciseController_Create_WithExistingExerciseName_ReturnsConflict()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new ExerciseController(mediator, mapper);

          
            var existingExercise = new Exercise { Name = "Existing Exercise", DurationInMinutes = 30, Type = ExerciseType.Yoga };
            await dbContext.Exercises.AddAsync(existingExercise);
            await dbContext.SaveChangesAsync();

            var exerciseDto = new ExerciseDto
            {
                Name = "Existing Exercise",
                DurationInMinutes = 45,
                Type = ExerciseType.Pilates
            };

            // Act
            var requestResult = await controller.Create(exerciseDto);

            // Assert
            var result = requestResult as ConflictObjectResult;
            Assert.NotNull(result);

            var errorMessage = result!.Value as string;
            Assert.NotNull(errorMessage);
            Assert.Equal("Exercise already exists: Exercise already exists", errorMessage);
        }

        [Fact]
        public async Task ExerciseController_Delete_WithExistingExerciseId_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

           
            contextBuilder.SeedExercises(3); 

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new ExerciseController(mediator, mapper);

            // Act
            var requestResult = await controller.Delete(1); 

            // Assert
            var result = requestResult as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task ExerciseController_Update_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();
            contextBuilder.SeedExercises(1);

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new ExerciseController(mediator, mapper);

            // Act
            var updateExerciseDto = new ExerciseDto
            {
                Id = 1,
                Name = "Updated Exercise Name",
                DurationInMinutes = 30,
                Type = ExerciseType.Yoga
            };
            var requestResult = await controller.Update(1, updateExerciseDto); 

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

       

    }
}