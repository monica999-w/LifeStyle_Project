using AutoMapper;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Work.WebApi;
using NSubstitute;


namespace LifeStyle.UnitTests.ControllerTests
{
    public class ExerciseControllerTests
    {

        private readonly IMapper _mapperMock;
        private readonly IMediator _mediatorMock;

        public ExerciseControllerTests()
        {
            _mapperMock = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
        }

        

        [Fact]
        public async Task GetExerciseById_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange

            var controller = new ExerciseController(_mediatorMock, _mapperMock);

            var exerciseId = 1;

            var expectedResult = new Exercise();

            _mediatorMock.Send(Arg.Any<GetExerciseById>()).Returns(expectedResult);
            _mapperMock.Map<ExerciseDto>(expectedResult).Returns(new ExerciseDto());

            // Act
            var result = await controller.GetExerciseById(exerciseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ExerciseDto>(okResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Create_ValidModelState_ReturnsOkResult()
        {
            var mediator = Substitute.For<IMediator>();
            var mapper = Substitute.For<IMapper>();
            // Arrange
            var controller = new ExerciseController(mediator, mapper);

            var exerciseDto = new ExerciseDto
            { Id = 1, Name = "Sample Exercise", DurationInMinutes = 30, Type = ExerciseType.Cardio };

            mapper.Map<ExerciseDto>(Arg.Any<Exercise>())
                .Returns(new ExerciseDto { Id= exerciseDto.Id,
                Name=exerciseDto.Name,
                DurationInMinutes=exerciseDto.DurationInMinutes,
                Type=exerciseDto.Type});

            // Act
            var result = await controller.Create(exerciseDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
         
        }

        [Fact]
        public async Task Delete_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange
            var controller = new ExerciseController(_mediatorMock, _mapperMock);
            var exerciseId = 1;
            var expectedResult = new Exercise();
            _mediatorMock.Send(Arg.Any<DeleteExercise>()).Returns(expectedResult);

            // Act
            var result = await controller.Delete(exerciseId);
           

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ExistingExerciseId_ReturnsOkResult()
        {
            // Arrange
            var controller = new ExerciseController(_mediatorMock, _mapperMock);
            var exerciseId = 1;
            var command = new UpdateExercise(exerciseId,"name",30, "descript", "video", ExerciseType.Yoga, Equipment.Machine, MajorMuscle.Back);
            var expectedResult = new Exercise();

            _mediatorMock.Send(Arg.Any<UpdateExercise>()).Returns(expectedResult);
            _mapperMock.Map<ExerciseDto>(expectedResult).Returns(new ExerciseDto());

            // Act
            var result = await controller.Update(exerciseId, new ExerciseDto { Id = command.ExerciseId, Name = command.Name, DurationInMinutes = command.DurationInMinutes, Type = command.Type });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ExerciseDto>(okResult.Value);
            Assert.NotNull(model);

        }
    }
}
