using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Responses;
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
        private readonly IMapper _mapperMock;

        public UpdateExerciseHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ExistingExerciseId_UpdatesExercise()
        {
            // Arrange
           
            var handler = new UpdateExerciseHandler(_unitOfWorkMock, _mapperMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Cardio);

            var existingExercise = new Exercise
            {
                ExerciseId = request.ExerciseId,
                Name = "Old Exercise",
                DurationInMinutes = 30,
                Type = ExerciseType.Pilates,
            };

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns(existingExercise);

            var updatedExerciseDto = new ExerciseDto 
            {
                Id = request.ExerciseId,
                Name = request.Name,
                DurationInMinutes = request.DurationInMinutes,
                Type = request.Type
            };

            _mapperMock.Map<ExerciseDto>(existingExercise).Returns(updatedExerciseDto);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedExerciseDto, result);

            // Verify that necessary methods are called
            await _unitOfWorkMock.Received(1).BeginTransactionAsync();
            await _unitOfWorkMock.Received(1).CommitTransactionAsync();
            await _unitOfWorkMock.ExerciseRepository.Received(1).Update(existingExercise);
            await _unitOfWorkMock.Received(1).SaveAsync();
        }

        [Fact]
        public async Task Handle_NonExistingExerciseId_ThrowsNotFoundException()
        {
            // Arrange

            var handler = new UpdateExerciseHandler(_unitOfWorkMock, _mapperMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Yoga); 

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns((Exercise)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));
        }

        [Fact]
        public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        {
            // Arrange
          
            var handler = new UpdateExerciseHandler(_unitOfWorkMock, _mapperMock);

            var request = new UpdateExercise(1, "Updated Exercise", 45, ExerciseType.Yoga);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Throws(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
        }
    }
}
