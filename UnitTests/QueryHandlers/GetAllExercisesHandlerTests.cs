using NSubstitute;
using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using NSubstitute.ExceptionExtensions;
using LifeStyle.Domain.Enums;

namespace LifeStyle.UnitTests.QueryHandlers
{
    public class GetAllExercisesHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
       

        public GetAllExercisesHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
           
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsListOfExercise()
        {
            // Arrange
            var handler = new GetAllExercisesHandler(_unitOfWorkMock);

            var exercises = new List<Exercise>() 
            {
              new Exercise { ExerciseId = 1, Name = "Exercise 1", DurationInMinutes = 30, Type = ExerciseType.Yoga },
              new Exercise { ExerciseId = 2, Name = "Exercise 2", DurationInMinutes = 45, Type = ExerciseType.Cardio }
            };


           _unitOfWorkMock.ExerciseRepository.GetAll().Returns(exercises);
            
            var request = new GetAllExercise();

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exercises.Count, result.Count);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_LogsAndThrowsException()
        {
            // Arrange

            var handler = new GetAllExercisesHandler(_unitOfWorkMock);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetAll().Throws(exception);

            var request = new GetAllExercise();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));

        }
    }

}


