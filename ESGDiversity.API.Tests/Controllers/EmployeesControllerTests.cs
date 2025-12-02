using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeService> _mockService;
    private readonly Mock<ILogger<EmployeesController>> _mockLogger;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _mockService = new Mock<IEmployeeService>();
        _mockLogger = new Mock<ILogger<EmployeesController>>();
        _controller = new EmployeesController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<EmployeeResponse>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 100,
            TotalPages = 10,
            Data = new List<EmployeeResponse>()
        };

        _mockService
            .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAll(1, 10, null);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<EmployeeResponse>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEmployeeExists()
    {
        // Arrange
        var employeeResponse = new EmployeeResponse
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@test.com",
            Department = "IT",
            Position = "Developer",
            Salary = 5000
        };

        _mockService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(employeeResponse);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<EmployeeResponse>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
