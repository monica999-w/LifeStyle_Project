using AutoMapper;
using LifeStyle.Application.Commands;
using LifeStyle.Application.Exercises.Query;
using LifeStyle.Application.Query;
using LifeStyle.Application.Responses;
using LifeStyle.Application.Users.Commands;
using LifeStyle.Application.Users.Query;
using LifeStyle.Controllers;
using LifeStyle.Domain.Enums;
using LifeStyle.Domain.Models.Exercises;
using LifeStyle.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;


namespace LifeStyle.UnitTests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly IMapper _mapperMock;
        private readonly IMediator _mediatorMock;

        public UserControllerTests()
        {
            _mapperMock = Substitute.For<IMapper>();
            _mediatorMock = Substitute.For<IMediator>();
        }

        [Fact]
        public async Task GetAllUser_Returns_Ok()
        {
            // Arrange

            var controller = new UsersController(_mediatorMock, _mapperMock);

            var expectedResult = new List<UserProfile>();

            _mediatorMock.Send(Arg.Any<GetAllUsers>()).Returns(expectedResult);
            _mapperMock.Map<List<UserDto>>(expectedResult).Returns(new List<UserDto>());


            // Act
            var result = await controller.GetAllUsers();

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<UserDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        }


        [Fact]
        public async Task GetUserById_ReturnsOkResult()
        {
            // Arrange

            var controller = new UsersController(_mediatorMock, _mapperMock);

            var exerciseId = 1;

            var expectedResult = new UserProfile();

            _mediatorMock.Send(Arg.Any<GetUserById>()).Returns(expectedResult);
            _mapperMock.Map<UserDto>(expectedResult).Returns(new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 });

            // Act
            var result = await controller.GetUserById(exerciseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task CreateUser_ValidModelState_ReturnsOkResult()
        {
           
            // Arrange
            var controller = new UsersController(_mediatorMock, _mapperMock);

            var userDto = new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 };

           
            _mapperMock.Map<UserDto>(Arg.Any<object>())
                .Returns(new UserDto { Id = userDto.Id, Email = userDto.Email, PhoneNumber = userDto.PhoneNumber, Height = userDto.Height, Weight = userDto.Weight });

            // Act
            var result = await controller.CreateUser(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task Delete_ExistingUserId_ReturnsOkResult()
        {
            // Arrange
            var controller = new UsersController(_mediatorMock, _mapperMock);
            var userId = 1;
            var expectedResult = new UserProfile();
            _mediatorMock.Send(Arg.Any<DeleteUser>()).Returns(expectedResult);

            // Act
            var result = await controller.DeleteUser(userId);


            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ExistingUserId_ReturnsOkResult()
        {
            // Arrange
            var controller = new UsersController(_mediatorMock, _mapperMock);
            var exerciseId = 1;
            var command = new UpdateUser(exerciseId, "Updated Exercise", "0757637653",170,89);

            var expectedResult = new UserProfile();

            _mediatorMock.Send(Arg.Any<UpdateUser>()).Returns(expectedResult);
            _mapperMock.Map<UserDto>(expectedResult).Returns(new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 });

            // Act
            var result = await controller.UpdateUser(exerciseId, new UserDto { Id = 1, Email = "Sample", PhoneNumber = "0764362883", Height = 170, Weight = 90 });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<UserDto>(okResult.Value);
            Assert.NotNull(model);

        }
    }
}

