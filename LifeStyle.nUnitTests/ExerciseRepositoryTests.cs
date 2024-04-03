using LifeStyle.LifeStyle.Aplication.Interfaces;
using LifeStyle.LifeStyle.Aplication.Logic;
using LifeStyle.LifeStyle.Domain.Enums;
using LifeStyle.LifeStyle.Domain.Models.Exercises;
using LifeStyle.LifeStyle.Domain.Models.Meal;
using LifeStyle.LifeStyle.Domain.Models.Users;
using Moq;


namespace LifeStyle.nUnitTests
{
    public class ExerciseRepositoryTests
    {

        [Fact]
        public async Task GetAll_ReturnsAllExercise()
        {
            // Arrange
            var exerciseRepository = new ExerciseRepository();

            // Act
            var result = await exerciseRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Exercise>>(result);
            Assert.Equal(3, ((List<Exercise>)result).Count);
        }

        [Fact]
        public async Task Add_Adds_New_Exercise()
        {
            var exerciseRepository = new InMemoryRepository<Exercise>();
            var newExercise = new Exercise(1, "test", 120, ExerciseType.Cardio);

            // Act
            await exerciseRepository.Add(newExercise);

            // Assert
            var result = await exerciseRepository.GetAll();
            Assert.Single(result);
            Assert.Contains(newExercise, result);
        }

        [Fact]
        public async Task Remove_RemoveExercise()
        {
          // Arrange
            var userRepository = new InMemoryRepository<UserProfile>();
            var userProfileToRemove = new UserProfile(1, "john@example.com", "123456789", 175, 70);
            await userRepository.Add(userProfileToRemove);

            // Act
            await userRepository.Remove(userProfileToRemove);

            // Assert
            var result = await userRepository.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public async Task Update_UpdatesExistingExercise()
        {
            // Arrange
            var userRepository = new InMemoryRepository<UserProfile>();
            var initialProfile = new UserProfile(1, "john@example.com", "123456789", 180, 75);
            await userRepository.Add(initialProfile);
            var updatedProfile = new UserProfile(2, "john@example.com", "123456789", 180, 75);

            // Act
            await userRepository.Update(updatedProfile);

            // Assert
            var result = (await userRepository.GetAll()).First();
            Assert.Equal(updatedProfile.Email, result.Email);
            Assert.Equal(updatedProfile.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedProfile.Height, result.Height);
            Assert.Equal(updatedProfile.Weight, result.Weight);
        }

    

        [Fact]
        public async Task GetById_ReturnsExerciseIfExists()
        {
            // Arrange
            var exerciseRepository = new ExerciseRepository();

            // Act
            var result = await exerciseRepository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Exercise>(result);
            Assert.Equal(1, result.Id);
        }
    }
}
