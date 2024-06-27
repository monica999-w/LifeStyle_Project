using AutoMapper;
using LifeStyle.Application.Meals.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Services;
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
        private readonly IFileService _fileServiceMock;

        public MealControllerTests()
        {
            _mapperMock = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
            _fileServiceMock = Substitute.For<IFileService>();
        }

        [Fact]
        public async Task GetMealById_ExistingMealId_ReturnsOkResult()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock, _fileServiceMock);
            var mealId = 1;
            var expectedResult = new Meal();
            _mediatorMock.Send(Arg.Any<GetMealById>()).Returns(expectedResult);
           
            // Act
            var result = await controller.GetMealById(mealId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<Meal>(okResult.Value);
        }

      

        [Fact]
        public async Task Delete_ExistingMealId_ReturnsNoContentResult()
        {
            // Arrange
            var controller = new MealController(_mediatorMock, _mapperMock, _fileServiceMock);
            var mealId = 1;

            // Act
            var result = await controller.DeleteMeal(mealId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
