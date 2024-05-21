using LifeStyle.Aplication.Interfaces;
using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Planners.Commands;
using LifeStyle.Domain.Exception;
using LifeStyle.Models.Planner;
using NSubstitute;


namespace LifeStyle.UnitTests.CommandHandlers
{
    public class DeletePlannerHandlerTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeletePlannerHandlerTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPlanner()
        {
            // Arrange
            var plannerRepository = Substitute.For<IPlannerRepository>();

            _unitOfWorkMock.PlannerRepository.Returns(plannerRepository);

            var handler = new DeletePlannerHandler(_unitOfWorkMock);

            var request = new DeletePlanner(1);
            var planner = new Planner { PlannerId = 1 };

            plannerRepository.GetPlannerById(Arg.Any<int>()).Returns(planner);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Planner>(result);
            await plannerRepository.Received(1).RemovePlanner(Arg.Any<Planner>());
            await _unitOfWorkMock.Received(1).SaveAsync();
        }

        [Fact]
        public async Task Handle_InvalidPlannerId_ThrowsNotFoundException()
        {
            // Arrange
            var plannerRepository = Substitute.For<IPlannerRepository>();

            _unitOfWorkMock.PlannerRepository.Returns(plannerRepository);

            var handler = new DeletePlannerHandler(_unitOfWorkMock);

            var request = new DeletePlanner(999);

            plannerRepository.GetPlannerById(Arg.Any<int>()).Returns((Planner)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
