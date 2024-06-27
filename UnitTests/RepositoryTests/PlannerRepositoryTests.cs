using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;
using Moq;

namespace LifeStyle.nUnitTests
{
    public class PlannerRepositoryTests
    {

        [Fact]
        public async Task AddPlanner_AddsNewPlanner()
        {
            // Arrange
            var userProfile = new UserProfile(3, "user3@example.com", "834875734", 165.0, 55.0);
            var planner = new Planner(userProfile);

            var mockPlannerRepository = new Mock<IPlannerRepository>();
            mockPlannerRepository.Setup(repo => repo.AddPlanner(It.IsAny<Planner>()))
                                 .ReturnsAsync(planner); 

            // Act
            var addedPlanner = await mockPlannerRepository.Object.AddPlanner(planner);

            // Assert
            Assert.Equal(planner, addedPlanner); 

            mockPlannerRepository.Setup(repo => repo.GetPlannerByUser(userProfile))
                                 .ReturnsAsync(planner);

            var result = await mockPlannerRepository.Object.GetPlannerByUser(userProfile);
            Assert.NotNull(result);
            Assert.Equal(planner, result);
        }
    }
}





