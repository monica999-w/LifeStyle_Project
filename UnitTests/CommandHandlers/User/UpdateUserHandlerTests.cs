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
    public class UpdateUserHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;


        public UpdateUserHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        }

        [Fact]
        public async Task Handle_ExistingUserId_Updates()
        {
            // Arrange

            var handler = new UpdateUserHandler(_unitOfWorkMock);

            var request = new UpdateUser(1, "Updated Exercise", "0764362883", 170,89);

            var sampleUser = new UserProfile { ProfileId = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 };

            _unitOfWorkMock.UserProfileRepository.GetById(request.UserId).Returns(sampleUser);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.UserId, result.ProfileId);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.PhoneNumber, result.PhoneNumber);
            Assert.Equal(request.Weight, result.Weight);
            Assert.Equal(request.Height, result.Height);
            await _unitOfWorkMock.UserProfileRepository.Received(1).Update(sampleUser);
            await _unitOfWorkMock.Received(1).SaveAsync();
            await _unitOfWorkMock.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task Handle_NonExistingUserId_ThrowsNotFoundException()
        {
            // Arrange

            var handler = new UpdateUserHandler(_unitOfWorkMock);

            var request = new UpdateUser(1, "Updated Exercise", "0764362883", 170, 89);

            _unitOfWorkMock.UserProfileRepository.GetById(request.UserId).Returns((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, default));
        }

        [Fact]
        public async Task Handle_ExceptionThrown_RollsBackTransactionAndThrowsException()
        {
            // Arrange

            var handler = new UpdateUserHandler(_unitOfWorkMock);

            var request = new UpdateUser(1, "Updated Exercise", "0764362883", 170, 89);

            var exception = new Exception("Test exception");
            _unitOfWorkMock.UserProfileRepository.GetById(request.UserId).Throws(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, default));
            await _unitOfWorkMock.Received(1).RollbackTransactionAsync();
        }
    }
}

