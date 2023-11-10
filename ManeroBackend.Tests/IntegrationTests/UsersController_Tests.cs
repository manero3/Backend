
using ManeroBackend.Authentication;
using ManeroBackend.Contexts;
using ManeroBackend.Controllers;
using ManeroBackend.Enums;
using ManeroBackend.Models;
using ManeroBackend.Models.DTOs;
using ManeroBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ManeroBackend.Tests.IntegrationTests;

public class UsersController_Tests
{
    [Fact]
    public async Task Register_User_With_Valid_UserRegistration_Dto_And_ReturnsOk()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var googleTokenServiceMock = new Mock<IGoogleTokenService>();
        var contextMock = new Mock<ApplicationDBContext>();
        var tokenServiceMock = new Mock<ITokenService>();

        var usersController = new UsersController(userServiceMock.Object, googleTokenServiceMock.Object, contextMock.Object, tokenServiceMock.Object);

        var registrationDto = new UserRegistrationDto
        {
            Email = "madicken@domain.com",
            Password = "BytMig1234!", 
            ConfirmPassword = "Test1234!",
            FirstName = "Madicken",
            LastName = "Eriksson"

        };

        // Configure mock responses
        userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                      .ReturnsAsync(new ServiceResponse<ApplicationUser> { Content = null });

        userServiceMock.Setup(x => x.CreateAsync(It.IsAny<ServiceRequest<UserRegistrationDto>>()))
                      .ReturnsAsync(new ServiceResponse<UserWithTokenResponse>
                      {
                          StatusCode = Enums.StatusCode.Created,
                          Content = new UserWithTokenResponse { User = new ApplicationUser(), Token = "TestToken" }
                      });

        // Act
        var result = await usersController.Register(registrationDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task Register_With_Already_ExistingUser_And_ReturnsConflict()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var googleTokenServiceMock = new Mock<IGoogleTokenService>();
        var contextMock = new Mock<ApplicationDBContext>();
        var tokenServiceMock = new Mock<ITokenService>();

        var usersController = new UsersController(userServiceMock.Object, googleTokenServiceMock.Object, contextMock.Object, tokenServiceMock.Object);

        var registrationDto = new UserRegistrationDto
        {
            Email = "madicken@domain.com",
            Password = "BytMig1234!",
            ConfirmPassword = "Test1234!",
            FirstName = "Madicken",
            LastName = "Eriksson"
        };

        // Configure mock responses
        userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                      .ReturnsAsync(new ServiceResponse<ApplicationUser> { Content = new ApplicationUser() });

        // Act
        var result = await usersController.Register(registrationDto);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, conflictResult.StatusCode);
        Assert.Equal("Email is already in use.", conflictResult.Value);
    }
}
