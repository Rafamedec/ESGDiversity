using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ILogger<AuthController>> _mockLogger;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_mockAuthService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "testpass"
        };

        var loginResponse = new LoginResponse
        {
            Token = "test-token",
            Username = "testuser",
            Role = "User"
        };

        _mockAuthService
            .Setup(s => s.LoginAsync(It.IsAny<LoginRequest>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<ActionResult<LoginResponse>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }

    [Fact]
    public async Task Register_ReturnsCreated_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "newuser",
            Email = "newuser@test.com",
            Password = "password123",
            Role = "User"
        };

        var loginResponse = new LoginResponse
        {
            Token = "test-token",
            Username = "newuser",
            Role = "User"
        };

        _mockAuthService
            .Setup(s => s.RegisterAsync(It.IsAny<RegisterRequest>()))
            .ReturnsAsync(loginResponse);

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        var createdResult = Assert.IsType<ActionResult<LoginResponse>>(result);
        var objectResult = Assert.IsType<CreatedAtActionResult>(createdResult.Result);
        Assert.Equal(201, objectResult.StatusCode);
    }
}
