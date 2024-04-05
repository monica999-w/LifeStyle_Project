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
        public async Task Add_Adds_New_Exercise()
        {
            // Arrange
            var exerciseRepositoryMock = new Mock<IRepository<Exercise>>();
            var exerciseRepository = exerciseRepositoryMock.Object;
            var newExercise = new Exercise(4, "testupdate", 124, ExerciseType.Cardio);

            // Act
            await exerciseRepository.Add(newExercise);

            // Assert
            exerciseRepositoryMock.Verify(repo => repo.Add(newExercise), Times.Once);
        }

        [Fact]
        public async Task Remove_RemoveExercise()
        {
            // Arrange
            var exercise = new List<Exercise>
        {
            new(7, "test1", 124, ExerciseType.Cardio)
        };
            var exerciseRepositoryMock = new Mock<IRepository<Exercise>>();
            exerciseRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(exercise.First());
            var exerciseRepository = exerciseRepositoryMock.Object;
            var exerciseToRemove = exercise.First();

            // Act
            await exerciseRepository.Remove(exerciseToRemove);

            // Assert
            exerciseRepositoryMock.Verify(repo => repo.Remove(exerciseToRemove), Times.Once);
        }

        [Fact]
        public async Task Update_UpdatesExistingExercise()
        {
            // Arrange
            var exercise = new List<Exercise>
        {
            new Exercise(7,"testupdate", 124, ExerciseType.Cardio)
        };
            var exerciseRepositoryMock = new Mock<IRepository<Exercise>>();
            exerciseRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(exercise.First());
            var exerciseRepository = exerciseRepositoryMock.Object;
            var updatedMeal = new Exercise(1, "test", 120, ExerciseType.Cardio);

            // Act
            await exerciseRepository.Update(updatedMeal);

            // Assert
            exerciseRepositoryMock.Verify(repo => repo.Update(updatedMeal), Times.Once);

        }

    }
}
