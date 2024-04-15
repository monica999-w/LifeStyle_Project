using LifeStyle.Aplication.Interfaces;
using LifeStyle.Aplication.Logic;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Meal;
using LifeStyle.Domain.Models.Users;
using Moq;


namespace LifeStyle.nUnitTests
{
    public class MealRepositoryTests
    {
       

        [Fact]
        public async Task Add_AddsNewMeal()
        {
            // Arrange
            var mealRepositoryMock = new Mock<IRepository<Meal>>();
            var mealRepository = mealRepositoryMock.Object;
            var meal = new Meal(7, "test1", MealType.Breakfast, new Nutrients(124, 32, 24, 24));

            // Act
            await mealRepository.Add(meal);

            // Assert
            mealRepositoryMock.Verify(repo => repo.Add(meal), Times.Once);
        }

        [Fact]
        public async Task Remove_RemoveMeal()
        {
            
            // Arrange
            var meal = new List<Meal>
        {
            new Meal(7, "test1",MealType.Breakfast, new Nutrients(124,32,24,24))
        };
            var mealRepositoryMock = new Mock<IRepository<Meal>>();
            mealRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(meal.First());
            var userRepository = mealRepositoryMock.Object;
            var userProfileToRemove = meal.First();

            // Act
            await userRepository.Remove(userProfileToRemove);

            // Assert
            mealRepositoryMock.Verify(repo => repo.Remove(userProfileToRemove), Times.Once);
        }

        [Fact]
        public async Task Update_Updates_Existing_Meal()
        {
            // Arrange
            var meal = new List<Meal>
        {
            new Meal(7, "test1",MealType.Breakfast, new Nutrients(124,32,24,24))
        };
            var mealRepositoryMock = new Mock<IRepository<Meal>>();
            mealRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(meal.First());
            var userRepository = mealRepositoryMock.Object;
            var updatedMeal = new Meal(1, "test1", MealType.Breakfast, new Nutrients(124, 32, 24, 24));

            // Act
            await userRepository.Update(updatedMeal);

            // Assert
            mealRepositoryMock.Verify(repo => repo.Update(updatedMeal), Times.Once);
        }


        [Fact]
        public async Task GetById_ReturnsMealIfExists()
        {
            // Arrange
            var meal = new List<Meal>
        {
            new(1, "test1", MealType.Breakfast, new Nutrients(124, 32, 24, 24))
        };
            var mealRepositoryMock = new Mock<IRepository<Meal>>();
            mealRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(meal.First());
            var userRepository = mealRepositoryMock.Object;

            // Act
            var result = await userRepository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(meal.First(), result);
        }
    }
}
