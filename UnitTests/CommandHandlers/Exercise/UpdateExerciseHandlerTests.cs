using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace LifeStyle.UnitTests.CommandHandlers
{
    public class UpdateExerciseHandlerTests
    {

        private readonly IUnitOfWork _unitOfWorkMock;
        

        public UpdateExerciseHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
         
        }

        [Fact]
        public async Task Handle_ExistingExerciseId_UpdatesExercise()
        {
            // Arrange

            var handler = new UpdateExerciseHandler(_unitOfWorkMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Cardio);

            var existingExercise = new Exercise
            {
                ExerciseId = request.ExerciseId,
                Name = "Old Exercise",
                DurationInMinutes = 30,
                Type = ExerciseType.Pilates,
            };

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns(existingExercise);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.ExerciseId, result.ExerciseId);
            Assert.Equal(request.Name, result.Name); 
            Assert.Equal(request.DurationInMinutes, result.DurationInMinutes); 
            Assert.Equal(request.Type, result.Type); 
            await _unitOfWorkMock.ExerciseRepository.Received(1).Update(existingExercise); 
            await _unitOfWorkMock.Received(1).SaveAsync(); 
            await _unitOfWorkMock.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task Handle_NonExistingExerciseId_ThrowsNotFoundException()
        {
            // Arrange

            var handler = new UpdateExerciseHandler(_unitOfWorkMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Yoga);

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns((Exercise)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));
        }

        [Fact]
        public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        {
            // Arrange

            var handler = new UpdateExerciseHandler(_unitOfWorkMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Yoga);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Throws(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
        }
    }
}
