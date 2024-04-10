using API.Core.DTOs;
using API.Core.Models;
using API.Core.Services;
using API.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace API.Test.Integration.Controllers;

public class UserControllerTests
{
    private UserController userController;
    private Mock<IUserService> mockUserService;
    private IConfiguration configuration;

    public UserControllerTests()
    {
        var configValues = new Dictionary<string, string>
            {
                {"Jwt:Key", "MyVeryVeryLongSuperSuperSecretKey"},
                {"Jwt:Issuer", "https://localhost:44322"},
                {"Jwt:Audience", "http://localhost:5173"},
            };

        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        mockUserService = new Mock<IUserService>();
        userController = new UserController(mockUserService.Object, configuration);
    }

    [Test]
    public void GetUser_CallGetUserInService()
    {
        int userId = 123;
        var expectedUser = new User();

        mockUserService.Setup(x => x.GetUser(userId)).Returns(expectedUser);

        var result = userController.GetUser(userId);

        mockUserService.Verify(x => x.GetUser(userId), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<User>(result);
        Assert.That(result, Is.EqualTo(expectedUser));
    }

    [Test]
    public void RegisterUser_CallCreateUserInService_GeneratesToken()
    {
        var newUserInfo = new UserDto() { Username = "admin", Password = "admin" };
        var createdUserResult = new User() { Username = newUserInfo.Username, CustomInstruction = "test", Id = 123 };

        mockUserService.Setup(x => x.CreateUser(newUserInfo)).Returns(createdUserResult);

        var result = userController.RegisterUser(newUserInfo);

        mockUserService.Verify(x => x.CreateUser(newUserInfo), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.TypeOf<string>().And.Not.Null.And.Not.Empty);
    }

    [Test]
    public void RegisterUser_CallCreateUserInService_UnauthorizedResult()
    {
        var newUserInfo = new UserDto() { Username = "admin", Password = "admin" };
        User createdUserResult = null;

        mockUserService.Setup(x => x.CreateUser(newUserInfo)).Returns(createdUserResult);

        var result = userController.RegisterUser(newUserInfo);

        mockUserService.Verify(x => x.CreateUser(newUserInfo), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public void Login_CallCreateUserInService_GeneratesToken()
    {
        var userInfo = new UserDto() { Username = "admin", Password = "admin" };
        var createdUserResult = new User() { Username = userInfo.Username, CustomInstruction = "test", Id = 123 };

        mockUserService.Setup(x => x.Login(userInfo)).Returns(createdUserResult);

        var result = userController.Login(userInfo);

        mockUserService.Verify(x => x.Login(userInfo), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.TypeOf<string>().And.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Login_CallCreateUserInService_UnauthorizedResult()
    {
        var userInfo = new UserDto() { Username = "admin", Password = "admin" };
        User createdUserResult = null;

        mockUserService.Setup(x => x.Login(userInfo)).Returns(createdUserResult);

        var result = userController.Login(userInfo);

        mockUserService.Verify(x => x.Login(userInfo), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public void UpdateUser_CallUpdateUserInService()
    {
        int userId = 123;
        int newRoleId = 456;

        var expectedUser = new User();

        mockUserService.Setup(x => x.UpdateUserRole(userId, newRoleId)).Returns(expectedUser);

        var result = userController.UpdateUser(userId, newRoleId);

        mockUserService.Verify(x => x.UpdateUserRole(userId, newRoleId), Times.Once);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<User>(result);
        Assert.That(result, Is.EqualTo(expectedUser));
    }

    [Test]
    public void DeleteUser_CallDeleteUserInService()
    {
        int userId = 123;

        userController.DeleteUser(userId);

        mockUserService.Verify(x => x.DeleteUser(userId), Times.Once);
    }

    [Test]
    public void ValidateUser_ReturnsOkResponse()
    {
        var result = userController.Validate();

        Assert.IsInstanceOf<OkResult>(result);
    }
}
