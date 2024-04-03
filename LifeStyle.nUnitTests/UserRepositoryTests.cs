﻿using LifeStyle.LifeStyle.Aplication.Interfaces;
using LifeStyle.LifeStyle.Aplication.Logic;
using LifeStyle.LifeStyle.Domain.Models.Users;
using Moq;


namespace LifeStyle.nUnitTests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetAll_Returns_All_UserProfiles()
        {
            // Arrange
            var userProfiles = new List<UserProfile>
        {
            new UserProfile(1, "john@example.com", "123456789", 175, 70),
            new UserProfile(2, "jane@example.com", "987654321", 160, 55),
            new UserProfile(3, "alice@example.com", "456123789", 180, 65)
        };
            var userRepositoryMock = new Mock<IRepository<UserProfile>>();
            userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(userProfiles);
            var userRepository = userRepositoryMock.Object;

            // Act
            var result = await userRepository.GetAll();

            // Assert
            Assert.Equal(userProfiles.Count, result.Count());
            Assert.Equal(userProfiles, result);
        }

        [Fact]
        public async Task Add_Adds_New_UserProfile()
        {
            // Arrange
            var userRepository = new InMemoryRepository<UserProfile>();
            var newUserProfile = new UserProfile(1, "new@example.com", "94863858", 160, 60);

            // Act
            await userRepository.Add(newUserProfile);

            // Assert
            var result = await userRepository.GetAll();
            Assert.Single(result);
            Assert.Contains(newUserProfile, result);
        }

        [Fact]
        public async Task Remove_Removes_Existing_UserProfile()
        {
            // Arrange
            var userProfiles = new List<UserProfile>
        {
            new UserProfile(1, "john@example.com", "123456789", 175, 70)
        };
            var userRepositoryMock = new Mock<IRepository<UserProfile>>();
            userRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(userProfiles.First());
            var userRepository = userRepositoryMock.Object;
            var userProfileToRemove = userProfiles.First();

            // Act
            await userRepository.Remove(userProfileToRemove);

            // Assert
            userRepositoryMock.Verify(repo => repo.Remove(userProfileToRemove), Times.Once);
        }

        [Fact]
        public async Task Update_Updates_Existing_UserProfile()
        {
            // Arrange
            var userProfiles = new List<UserProfile>
        {
            new UserProfile(1, "john@example.com", "123456789", 175, 70)
        };
            var userRepositoryMock = new Mock<IRepository<UserProfile>>();
            userRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(userProfiles.First());
            var userRepository = userRepositoryMock.Object;
            var updatedUserProfile = new UserProfile(1, "updated@example.com", "newpassword", 180, 75);

            // Act
            await userRepository.Update(updatedUserProfile);

            // Assert
            userRepositoryMock.Verify(repo => repo.Update(updatedUserProfile), Times.Once);
        }

        [Fact]
        public async Task GetById_Returns_UserProfile_IfExists()
        { // Arrange
            var userRepository = new InMemoryRepository<UserProfile>();
            var userProfile = new UserProfile(1, "john@example.com", "123456789", 180, 75);
            await userRepository.Add(userProfile);

            // Act
            var result = await userRepository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userProfile, result);
        }
    }
}