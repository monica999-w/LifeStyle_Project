using AutoMapper;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeStyle.UnitTests.ControllerTests
{
    public class MealControllerTests
    {
        private readonly IMapper _mapperMock;
        private readonly IMediator _mediatorMock;

        public MealControllerTests()
        {
            _mapperMock = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
        }

        [Fact]
        public async Task GetAllMeals_Returns_Ok()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock);
            var expectedResult = new List<Meal>();
            _mediatorMock.Send(Arg.Any<GetAllMeals>()).Returns(expectedResult);
            _mapperMock.Map<List<MealDto>>(expectedResult).Returns(new List<MealDto>());

            // Act
            var result = await controller.GetAllMeals();

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<MealDto>>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetMealById_ExistingMealId_ReturnsOkResult()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock);
            var mealId = 1;
            var expectedResult = new Meal();
            _mediatorMock.Send(Arg.Any<GetMealById>()).Returns(expectedResult);
            _mapperMock.Map<MealDto>(expectedResult).Returns(new MealDto());

            // Act
            var result = await controller.GetMealById(mealId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<MealDto>(okResult.Value);
        }

        [Fact]
        public async Task Create_ValidModelState_ReturnsOkResult()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock);
            var mealDto = new MealDto { Id = 1, Name = "Sample Meal", MealType = MealType.Breakfast, Nutrients = new Nutrients() };

            // Act
            var result = await controller.CreateMeal(mealDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingMealId_ReturnsNoContentResult()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock);
            var mealId = 1;

            // Act
            var result = await controller.DeleteMeal(mealId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
