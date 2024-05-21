using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Infrastructure.UnitOfWork;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;

namespace LifeStyle.UnitTests.CommandHandlers
{
    public class DeleteExerciseHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
       

        public DeleteExerciseHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
           
        }

        [Fact]
        public async Task Handle_ExistingExerciseId_DeletesExercise()
        {
            // Arrange
            var handler = new DeleteExerciseHandler(_unitOfWorkMock);
            var exerciseId = 1;

            var existingExercise = new Exercise
            {
                ExerciseId = exerciseId,
                Name = "Exercise 1",
                DurationInMinutes = 30,
                Type = ExerciseType.Yoga
            };
            _unitOfWorkMock.ExerciseRepository.GetById(exerciseId).Returns(existingExercise); 

            // Act
            var result = await handler.Handle(new DeleteExercise(exerciseId), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingExercise, result); 
            await _unitOfWorkMock.ExerciseRepository.Received(1).Remove(existingExercise);
            await _unitOfWorkMock.Received(1).SaveAsync(); 
            await _unitOfWorkMock.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task Handle_NonExistingExerciseId_ThrowsNotFoundException()
        {
            // Arrange


            var handler = new DeleteExerciseHandler(_unitOfWorkMock);

            var request = new DeleteExercise(1);

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns((Exercise)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));


        }

        [Fact]
        public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        {
            // Arrange
            var handler = new DeleteExerciseHandler(_unitOfWorkMock);
            var request = new DeleteExercise(1);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Throws(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));


            await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
        }
    }
}
