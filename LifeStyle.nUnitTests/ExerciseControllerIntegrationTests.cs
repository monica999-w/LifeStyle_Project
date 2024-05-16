using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Moq;
using System.Net;


namespace LifeStyle.IntegrationTests
{
    public class ExerciseControllerIntegrationTests
    {
        [Fact]
        public async Task ExerciseController_CreateExercise_ReturnCreatedExercise()
        {
            // Arrange
            var exerciseDto = new ExerciseDto
            {
                Name = "Test Exercise",
                DurationInMinutes = 30,
                Type = ExerciseType.Cardio
            };

            using var contextBuilder = new DataContextBuilder();
            var dbContext = contextBuilder.GetContext();

            var mediator = TestHelpers.CreateMediator(dbContext);
            var controller = new ExerciseController(mediator);

            // Act
            var requestResult = await controller.Create(exerciseDto);

            // Assert
            var result = requestResult as OkObjectResult;
            var createdExercise = result!.Value as ExerciseDto;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            Assert.NotNull(createdExercise);
            Assert.Equal(exerciseDto.Name, createdExercise.Name);
            Assert.Equal(exerciseDto.DurationInMinutes, createdExercise.DurationInMinutes);
            Assert.Equal(exerciseDto.Type, createdExercise.Type);
        }
    }
}
