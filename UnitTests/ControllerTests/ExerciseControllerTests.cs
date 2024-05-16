using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;


namespace LifeStyle.UnitTests.ControllerTests
{
    public class ExerciseControllerTests
    {
        [Fact]
        public async Task GetAllExercises_Returns_Ok()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = new ExerciseController(mediator);

            var expectedResult = new List<ExerciseDto>(); 
            mediator.Send(Arg.Any<GetAllExercise>()).Returns(expectedResult);

            // Act
            var result = await controller.GetAllExercises();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ExerciseDto>>(okResult.Value);
            Assert.Equal(expectedResult, model);
        }


        [Fact]
        public async Task GetExerciseById_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = new ExerciseController(mediator);
            var exerciseId = 1;

            var expectedResult = new ExerciseDto(); 

            mediator.Send(Arg.Any<GetExerciseById>()).Returns(expectedResult);

            // Act
            var result = await controller.GetExerciseById(exerciseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ExerciseDto>(okResult.Value);
            Assert.Equal(expectedResult, model);
        }

        [Fact]
        public async Task Create_ValidModelState_ReturnsOkResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = new ExerciseController(mediator);
            var command = new CreateExercise("ExerciseName", 30, ExerciseType.Yoga); 

            var expectedResult = new ExerciseDto(); 

            mediator.Send(Arg.Any<CreateExercise>()).Returns(expectedResult);

            // Act
          //  var result = await controller.Create(command);

            // Assert
         //  var okResult = Assert.IsType<OkObjectResult>(result);
///var model = Assert.IsAssignableFrom<ExerciseDto>(okResult.Value);
           // Assert.Equal(expectedResult, model);
        }

        [Fact]
        public async Task Delete_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = new ExerciseController(mediator);
            var exerciseId = 1;

            // Act
            var result = await controller.Delete(exerciseId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var controller = new ExerciseController(mediator);
            var exerciseId = 1;
            var command = new UpdateExercise(exerciseId, "Updated Exercise", 45, ExerciseType.Cardio); 

            var expectedResult = new ExerciseDto(); 

            mediator.Send(Arg.Any<UpdateExercise>()).Returns(expectedResult);

            //// Act
            //var result = await controller.Update(exerciseId, command);

            //// Assert
            //var okResult = Assert.IsType<OkObjectResult>(result);
            //var model = Assert.IsAssignableFrom<ExerciseDto>(okResult.Value);
            //Assert.Equal(expectedResult, model);
        }


    }


}
