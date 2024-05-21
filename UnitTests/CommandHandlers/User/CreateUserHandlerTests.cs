using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Commands;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Users;
using NSubstitute;


namespace LifeStyle.UnitTests.CommandHandlers.User
{
    public class CreateUserHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateUserHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUser()
        {
            //Arrange
            var repository = Substitute.For<IRepository<UserProfile>>();

            var handler = new CreateUserHander(_unitOfWorkMock);

            var request = new CreateUser("email@email.com", "0745274633",120,90);

            repository?.GetByName(Arg.Any<string>()).Returns((UserProfile)null);
            _unitOfWorkMock.UserProfileRepository.Returns(repository);

            repository.When(repo => repo.Add(Arg.Any<UserProfile>()))
               .Do(info =>
               {
                   var exerciseArg = info.Arg<UserProfile>();
                   Assert.Equal(request.Email, exerciseArg.Email);
                   Assert.Equal(request.PhoneNumber, exerciseArg.PhoneNumber);
                   Assert.Equal(request.Height, exerciseArg.Height);
                   Assert.Equal(request.Weight, exerciseArg.Weight);
               });

            // Act
            var result = await handler.Handle(request, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserProfile>(result);
            await repository.Received(1).Add(Arg.Any<UserProfile>());
            await _unitOfWorkMock.Received(1).SaveAsync();
        }


        [Fact]
        public async Task Handle_ExistingUser_ThrowsAlreadyExistsException()
        {
            // Arrange
           
            var handler = new CreateUserHander(_unitOfWorkMock);
            var request = new CreateUser("email@email.com", "0745274633", 120, 90);

            var existingUser = new UserProfile();
            _unitOfWorkMock.UserProfileRepository.GetByName(request.Email).Returns(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(request, default));


        }
    }
}


