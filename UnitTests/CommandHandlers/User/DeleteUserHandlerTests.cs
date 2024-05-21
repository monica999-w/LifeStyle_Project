using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Users.Commands;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using NSubstitute;
using NSubstitute.ExceptionExtensions;


namespace LifeStyle.UnitTests.CommandHandlers.User
{
    public class DeleteUserHandlerTests
    {
        public class DeleteExerciseHandlerTests
        {
            private readonly IUnitOfWork _unitOfWorkMock;


            public DeleteExerciseHandlerTests()
            {
                _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            }

            [Fact]
            public async Task Handle_ExistingUserId_Delete()
            {
                // Arrange
                var handler = new DeleteUserHandler(_unitOfWorkMock);
                var userId = 1;

                var sampleUser = new UserProfile { ProfileId = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 };
                _unitOfWorkMock.UserProfileRepository.GetById(userId).Returns(sampleUser);

                // Act
                var result = await handler.Handle(new DeleteUser(userId), default);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(sampleUser, result);
                await _unitOfWorkMock.UserProfileRepository.Received().Remove(sampleUser);
                await _unitOfWorkMock.Received(1).SaveAsync();
                await _unitOfWorkMock.Received(1).CommitTransactionAsync();
            }

            [Fact]
            public async Task Handle_NonExistingUserId_ThrowsNotFoundException()
            {
                // Arrange


                var handler = new DeleteUserHandler(_unitOfWorkMock);

                var request = new DeleteUser(1);

                _unitOfWorkMock.UserProfileRepository.GetById(request.UserId).Returns((UserProfile)null);

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));


            }

            [Fact]
            public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
            {
                // Arrange
                var handler = new DeleteUserHandler(_unitOfWorkMock);
                var request = new DeleteUser(1);


                var exception = new Exception("Test exception");
                _unitOfWorkMock.UserProfileRepository.GetById(request.UserId).Throws(exception);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));


                await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
            }
        }


    }
}
