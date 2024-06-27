using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Commands;
using LifeStyle.Domain.Exception;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;

using NSubstitute;

namespace LifeStyle.UnitTests.CommandHandlers
{
    public class CreatePlannerHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;


        public CreatePlannerHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        }

        [Fact]
        public async Task Handle_InvalidUserId_ThrowsNotFoundException()
        {
            // Arrange
            var userProfileRepository = Substitute.For<IRepository<UserProfile>>();

            _unitOfWorkMock.UserProfileRepository.Returns(userProfileRepository);

            var handler = new CreatePlannerHandler(_unitOfWorkMock);

            var plannedDate = DateTime.UtcNow;
            var request = new CreatePlanner("email",plannedDate, new List<int> { 1, 2 }, new List<int> { 3, 4 });

            // Stub metoda GetById pentru a returna null
            userProfileRepository.GetById(Arg.Any<int>()).Returns((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}


