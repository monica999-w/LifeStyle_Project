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
            mockPlannerRepository.Setup(repo => repo.AddPlanner(planner))
                                 .Returns(Task.CompletedTask);

            // Act
            await mockPlannerRepository.Object.AddPlanner(planner);

            // Assert
           
            mockPlannerRepository.Setup(repo => repo.GetPlannerByUser(userProfile))
                                 .Returns(Task.FromResult(planner));

            var result = await mockPlannerRepository.Object.GetPlannerByUser(userProfile);
            Assert.NotNull(result); 
            Assert.Equal(planner, result); 
        }


        [Fact]
        public async Task RemovePlanner_RemovesPlanner()
        {
            // Arrange
            var userProfile = new UserProfile(1, "user1@example.com", "123456789", 170.0, 70.0);
            var plannerToRemove = new Planner(userProfile);

            var mockPlannerRepository = new Mock<IPlannerRepository>();
            mockPlannerRepository.Setup(repo => repo.RemovePlanner(plannerToRemove))
                                 .Returns(Task.CompletedTask);

            // Act
            await mockPlannerRepository.Object.RemovePlanner(plannerToRemove);

            // Assert
            var result = await mockPlannerRepository.Object.GetPlannerByUser(userProfile);
            Assert.Null(result);
        }
    }
}





