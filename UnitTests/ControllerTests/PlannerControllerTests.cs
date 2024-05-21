using AutoMapper;
using LifeStyle.Application.Planners.Commands;
using LifeStyle.Application.Planners.Query;
using LifeStyle.Application.Planners.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Models.Planner;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LifeStyle.UnitTests.ControllerTests
{
    public class PlannerControllerTests
    {
        private readonly IMapper _mapperMock;
        private readonly IMediator _mediatorMock;

        public PlannerControllerTests()
        {
            _mapperMock = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
        }

        [Fact]
        public async Task GetAllPlanners_Returns_Ok()
        {
            // Arrange
            var controller = new PlannerController(_mediatorMock, _mapperMock);
            var expectedResult = new List<Planner>();
            _mediatorMock.Send(Arg.Any<GetAllPlanners>()).Returns(expectedResult);
            _mapperMock.Map<List<PlannerDto>>(expectedResult).Returns(new List<PlannerDto>());

            // Act
            var result = await controller.GetAllPlanners();

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<PlannerDto>>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreatePlanner_ValidModelState_ReturnsOkResult()
        {
            // Arrange
            var controller = new PlannerController(_mediatorMock, _mapperMock);
            var plannerDto = new PlannerDto { ProfileId = 1, MealIds = new List<int>(), ExerciseIds = new List<int>() };
            var expectedResult = new Planner { PlannerId = 1, Meals = new List<Meal>(), Exercises = new List<Exercise>() };
            _mediatorMock.Send(Arg.Any<CreatePlanner>()).Returns(expectedResult);
            _mapperMock.Map<PlannerDto>(expectedResult).Returns(new PlannerDto());

            // Act
            var result = await controller.CreatePlanner(plannerDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeletePlanner_ExistingPlannerId_ReturnsNoContentResult()
        {
            // Arrange
            var controller = new PlannerController(_mediatorMock, _mapperMock);
            var plannerId = 1;
            var expectedResult = new Planner();
            _mediatorMock.Send(Arg.Any<DeletePlanner>()).Returns(expectedResult);

            // Act
            var result = await controller.DeletePlanner(plannerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
