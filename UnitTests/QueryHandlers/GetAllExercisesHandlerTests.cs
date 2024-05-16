using NSubstitute;
using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Domain.Models.Exercises;
using NSubstitute.ExceptionExtensions;

namespace LifeStyle.UnitTests.QueryHandlers
{
    public class GetAllExercisesHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapperMock;

        public GetAllExercisesHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsListOfExerciseDto()
        {
            // Arrange
            var handler = new GetAllExercisesHandler(_unitOfWorkMock, _mapperMock);

            var exercises = new List<Exercise>(); 
            var exerciseDtos = new List<ExerciseDto>();

            _unitOfWorkMock.ExerciseRepository.GetAll().Returns(exercises);
            _mapperMock.Map<List<ExerciseDto>>(exercises).Returns(exerciseDtos);

            var request = new GetAllExercise();

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            Assert.Equal(exerciseDtos, result);
        }
        [Fact]
        public async Task Handle_ExceptionThrown_LogsAndThrowsException()
        {
            // Arrange
          
            var handler = new GetAllExercisesHandler(_unitOfWorkMock, _mapperMock);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetAll().Throws(exception);

            var request = new GetAllExercise();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));

        }
    }

}


