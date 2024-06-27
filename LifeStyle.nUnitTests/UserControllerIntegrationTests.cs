using LifeStyle.Aplication.Logic;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Users.Query;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Repository;
using LifeStyle.Infrastructure.UnitOfWork;
using LifeStyle.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;

using System.Net;



namespace LifeStyle.IntegrationTests
{
    public class UserControllerIntegrationTests
    {
        [Fact]
        public async Task UserController_GetAll_ReturnsOkWithCorrectData()
        {// Arrange
            var numberOfUser = 3;

            using var contextBuilder = new DataContextBuilder();
            contextBuilder.SeedUsers(numberOfUser);
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

            var controller = new UsersController(mediator, mapper);

            // Act
            var requestResult = await controller.GetAllUsers();

            // Assert
            var result = requestResult.Result as OkObjectResult;
            var user = result!.Value as List<UserDto>;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            Assert.NotNull(user);
            Assert.Equal(numberOfUser, user.Count);
        }

        [Fact]
        public async Task UserController_GetUserById_WithExistingId_ReturnsOkWithCorrectData()
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

            var controller = new UsersController(mediator, mapper);

            
            var sampleUser = new UserProfile { ProfileId = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight= 90 ,UserId="10"};
            await dbContext.UserProfiles.AddAsync(sampleUser);
            await dbContext.SaveChangesAsync();

            // Act
            var requestResult = await controller.GetUserById(1);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);

            var userDto = result!.Value as UserDto;
            Assert.NotNull(userDto);

        }

        [Fact]
        public async Task UserController_Create_WithValidData_ReturnsOk()
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

            var controller = new UsersController(mediator, mapper);

            // Act
           var sampleUser = new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight= 90 };
            var requestResult = await controller.CreateUser(sampleUser);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }


        [Fact]
        public async Task UserController_Delete_ReturnsNoContent()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            contextBuilder.SeedUsers(3);

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new UsersController(mediator, mapper);

            // Act
            var requestResult = await controller.DeleteUser(1);

            // Assert
            var result = requestResult as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task UserController_Update_WithValidData_ReturnsOk()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();
            contextBuilder.SeedUsers(1);

            var exerciseRepository = new ExerciseRepository(dbContext);
            var nutrientRepository = new NutrientRepository(dbContext);
            var mealRepository = new MealRepository(dbContext);
            var plannerRepository = new PlannerRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext, exerciseRepository, nutrientRepository, mealRepository, userRepository, plannerRepository);

            var mapper = TestHelpers.CreateMapper();
            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new UsersController(mediator, mapper);

            // Act
            var sampleUserDto = new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 };


            var requestResult = await controller.UpdateUser(1, sampleUserDto);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }



    }
}
    
