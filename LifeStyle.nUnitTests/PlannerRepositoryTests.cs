using LifeStyle.LifeStyle.Aplication.Logic;
using LifeStyle.LifeStyle.Domain.Models.Users;
using LifeStyle.Models.Planner;

namespace LifeStyle.nUnitTests
{
    public  class PlannerRepositoryTests
    {

        [Fact]
        public async Task AddPlanner_AddsNewPlanner()
        {
            // Arrange
            var plannerRepository = new PlannerRepository();
            var userProfile = new UserProfile(3, "user3@example.com", "834875734", 165.0, 55.0);
            var planner = new Planner(userProfile);

            // Act
            await plannerRepository.AddPlanner(planner);

            // Assert
            var result = await plannerRepository.GetPlannerByUser(userProfile);
            Assert.NotNull(result);
            Assert.Equal(planner, result);
        }

        [Fact]
        public async Task RemovePlanner_RemovesPlanner()
        {
            // Arrange
            var plannerRepository = new PlannerRepository();
            var userProfile = new UserProfile(1, "user1@example.com", "123456789", 170.0, 70.0);
            var plannerToRemove = await plannerRepository.GetPlannerByUser(userProfile);

            // Act
            await plannerRepository.RemovePlanner(plannerToRemove);

            // Assert
            var result = await plannerRepository.GetPlannerByUser(userProfile);
            Assert.Null(result);
        }

        

    }
}

