using DiskodePro.WebApi.Controllers;
using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DiskodePro.WebApi.UT.ControllerTests;

[TestFixture]
public class UserControllerTests
{
    private static bool IsSameActualAndExpectedUser(User actual, User expected)
    {
        return actual.Password == expected.Password
               && actual.Email == expected.Email
               && actual.Name == expected.Name
               && actual.UserId == expected.UserId;
    }

    [Test]
    public async Task GetUserShouldReturnCorrectUser()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var expectedUser = new User
        {
            UserId = 1,
            Name = "User Name",
            Email = "user@example.com",
            Password = "12345678"
        };
        mockUserRepository.Setup(
                userRepository => userRepository.GetUserByIdAsync(expectedUser.UserId))
            .ReturnsAsync(expectedUser);
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.GetUser(expectedUser.UserId);
        var actualUser = actionResult.Result as OkObjectResult;

        // Assert
        Assert.That(actualUser, Is.Not.Null);
        Assert.That(actualUser!.Value, Is.Not.Null);
        Assert.That(IsSameActualAndExpectedUser((User)actualUser.Value!, expectedUser));
    }

    [Test]
    public async Task GetUsersShouldReturnAllUsers()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var expectedUsers = new List<User>
        {
            new()
            {
                UserId = 1,
                Name = "User Name",
                Email = "user@example.com",
                Password = "12345678"
            },
            new()
            {
                UserId = 2,
                Name = "User Name",
                Email = "user@example.com",
                Password = "12345678"
            }
        };
        mockUserRepository.Setup(
                userRepository => userRepository.GetAllUsersAsync())
            .ReturnsAsync(expectedUsers);
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.GetUsers();
        var okObjectResult = actionResult.Result as OkObjectResult;
        var actualUsers = okObjectResult?.Value as List<User>;

        // Assert
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(actualUsers, Is.Not.Null);
        Assert.That(actualUsers!, Has.Count.EqualTo(expectedUsers.Count));
        for (var i = 0; i < expectedUsers.Count; i++)
            Assert.That(IsSameActualAndExpectedUser(actualUsers![i], expectedUsers[i]));
    }

    [Test]
    public async Task GetUserReturnsNotFoundWhenUserNotFound()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(
                userRepository => userRepository.GetUserByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new UserNotFoundException(""));
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.GetUser(1);

        // Assert
        Assert.That(actionResult.Result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }

    [Test]
    public async Task CreateUserShouldReturnCreatedAtActionResult()
    {
        // Arrange
        var userDto = new UserDTO
        {
            Name = "User Name",
            Email = "user@example.com",
            Password = "12345678"
        };
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(
                userRepository => userRepository.CreateUserAsync(userDto))
            .ReturnsAsync(new User
            {
                UserId = 1,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password
            });
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.CreateUser(userDto);
        var createdAction = actionResult.Result as CreatedAtActionResult;
        var createdUser = createdAction?.Value as User;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(createdAction, Is.Not.Null);
        Assert.That(createdUser, Is.Not.Null);
    }

    [Test]
    public async Task CreateUserSendsConflictForDuplicateEmail()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(
                userRepository => userRepository.CreateUserAsync(It.IsAny<UserDTO>()))
            .ThrowsAsync(new DuplicateEmailException("", new Exception()));
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.CreateUser(new UserDTO
        {
            Email = "taken@example.com",
            Name = "",
            Password = "12345678"
        });

        // Assert
        Assert.That(actionResult.Result, Is.InstanceOf(typeof(ConflictObjectResult)));
    }

    [Test]
    public async Task UpdateUserUpdateCorrectly()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        const int userId = 1;
        var userDto = new UserDTO
        {
            Email = "user@example.com",
            Name = "user name",
            Password = "password"
        };
        var expectedUser = new User
        {
            UserId = userId,
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password
        };
        mockUserRepository.Setup(
                userRepository => userRepository.UpdateUserAsync(userId, userDto))
            .ReturnsAsync(expectedUser);
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.UpdateUser(userId, userDto);
        var actualUser = actionResult.Result as OkObjectResult;

        // Assert
        Assert.That(actualUser, Is.Not.Null);
        Assert.That(actualUser!.Value, Is.Not.Null);
        Assert.That(IsSameActualAndExpectedUser((User)actualUser.Value!, expectedUser));
    }

    [Test]
    public async Task UpdateUserSendsConflictForDuplicateEmail()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(
                userRepository => userRepository
                    .UpdateUserAsync(It.IsAny<int>(), It.IsAny<UserDTO>()))
            .ThrowsAsync(new DuplicateEmailException("", new Exception()));
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.UpdateUser(1, new UserDTO
        {
            Email = "taken@example.com",
            Name = "",
            Password = "12345678"
        });

        // Assert
        Assert.That(actionResult.Result, Is.InstanceOf(typeof(ConflictObjectResult)));
    }

    [Test]
    public async Task UpdateUserSendsNotFoundForIncorrectUserId()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(userRepository => userRepository
                .UpdateUserAsync(It.IsAny<int>(), It.IsAny<UserDTO>()))
            .ThrowsAsync(new UserNotFoundException(""));
        var userController = new UserController(mockUserRepository.Object);

        // Act
        var actionResult = await userController.UpdateUser(1, new UserDTO
        {
            Email = "taken@example.com",
            Name = "",
            Password = "12345678"
        });

        // Assert
        Assert.That(actionResult.Result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }

    [Test]
    public async Task DeleteUserSendsCorrectResponse()
    {
        //Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(
            userRepository => userRepository.DeleteUserAsync(It.IsAny<int>()));
        var userController = new UserController(mockUserRepository.Object);

        //Act
        var result = await userController.DeleteUser(1);

        //Assert
        Assert.That(result, Is.InstanceOf(typeof(NoContentResult)));
    }

    [Test]
    public async Task DeleteUserSendsNotFoundForIncorrectUserId()
    {
        //Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(userRepository => userRepository.DeleteUserAsync(It.IsAny<int>()))
            .ThrowsAsync(new UserNotFoundException(""));
        var userController = new UserController(mockUserRepository.Object);

        //Act
        var result = await userController.DeleteUser(1);

        //Assert
        Assert.That(result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }
}