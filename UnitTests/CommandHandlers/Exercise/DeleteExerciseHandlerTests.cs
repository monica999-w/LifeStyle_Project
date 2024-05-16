using AutoMapper;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;

namespace LifeStyle.UnitTests.CommandHandlers
{
    public class DeleteExerciseHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapperMock;

        public DeleteExerciseHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _mapperMock = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ExistingExerciseId_DeletesExercise()
        {
            // Arrange
            var handler = new DeleteExerciseHandler(_unitOfWorkMock, _mapperMock);
            var request = new DeleteExercise(1); 

            var existingExercise = new Exercise(); 
            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns(existingExercise);

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            Assert.Equal(Unit.Value, result);

            // Verify that necessary methods are called
            await _unitOfWorkMock.Received(1).BeginTransactionAsync();
            await _unitOfWorkMock.Received(1).CommitTransactionAsync();
            await _unitOfWorkMock.ExerciseRepository.Received(1).Remove(existingExercise);
            await _unitOfWorkMock.Received(1).SaveAsync();
        }

        [Fact]
        public async Task Handle_NonExistingExerciseId_ThrowsNotFoundException()
        {
            // Arrange
            

            var handler = new DeleteExerciseHandler(_unitOfWorkMock, _mapperMock);

            var request = new DeleteExercise(1); 

            _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Returns((Exercise)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request,default));

           
        }

        //[Fact]
        //public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        //{
        //    // Arrange
        //    var handler = new DeleteExerciseHandler(_unitOfWorkMock, _mapperMock);
        //    var request = new DeleteExercise(1); // Sample valid request

        //    var exception = new Exception("Test exception");
        //    _unitOfWorkMock.ExerciseRepository.GetById(request.ExerciseId).Throws(exception);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

            
        //    await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
        //}
    }
}
