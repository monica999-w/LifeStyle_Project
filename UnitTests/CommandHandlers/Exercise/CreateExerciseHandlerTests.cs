using AutoMapper;
using LifeStyle.Aplication.Interfaces;
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
    public class CreateExerciseHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
       
        public CreateExerciseHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
           
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsExercise()
        {
            //Arrange
            var repository = Substitute.For<IRepository<Exercise>>();

            var handler = new CreateExerciseHandler(_unitOfWorkMock);

            var request = new CreateExercise("Exercise 1", 30, ExerciseType.Yoga);

            repository?.GetByName(Arg.Any<string>()).Returns((Exercise)null);
            _unitOfWorkMock.ExerciseRepository.Returns(repository);

            repository.When(repo => repo.Add(Arg.Any<Exercise>()))
               .Do(info =>
               {
                   var exerciseArg = info.Arg<Exercise>();
                   Assert.Equal(request.Name, exerciseArg.Name);
                   Assert.Equal(request.DurationInMinutes, exerciseArg.DurationInMinutes);
                   Assert.Equal(request.Type, exerciseArg.Type);
               });

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Exercise>(result); 
            await repository.Received(1).Add(Arg.Any<Exercise>());
            await _unitOfWorkMock.Received(1).SaveAsync();
        }


        [Fact]
        public async Task Handle_ExistingExerciseName_ThrowsAlreadyExistsException()
        {
           // Arrange
           var handler = new CreateExerciseHandler(_unitOfWorkMock);
            var request = new CreateExercise("ExistingExercise", 30, ExerciseType.Yoga);

            var existingExercise = new Exercise();
            _unitOfWorkMock.ExerciseRepository.GetByName(request.Name).Returns(existingExercise);

           // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, default));


        }

        [Fact]
        public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        {
           // Arrange
           var handler = new CreateExerciseHandler(_unitOfWorkMock);
            var request = new CreateExercise("ExerciseName", 30, ExerciseType.Yoga);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.ExerciseRepository.GetByName(request.Name).Throws(exception);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));


        }
    }
}

