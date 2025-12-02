using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class DiversityGoalsControllerTests
{
    private readonly Mock<IDiversityGoalService> _mockService;
    private readonly Mock<ILogger<DiversityGoalsController>> _mockLogger;
    private readonly DiversityGoalsController _controller;

    public DiversityGoalsControllerTests()
    {
        _mockService = new Mock<IDiversityGoalService>();
        _mockLogger = new Mock<ILogger<DiversityGoalsController>>();
        _controller = new DiversityGoalsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<DiversityGoalResponse>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 20,
            TotalPages = 2,
            Data = new List<DiversityGoalResponse>()
        };

        _mockService
            .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetAll(1, 10);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<DiversityGoalResponse>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenGoalExists()
    {
        // Arrange
        var goalResponse = new DiversityGoalResponse
        {
            Id = 1,
            Department = "IT",
            MetricType = "Gender",
            TargetPercentage = 50,
            CurrentPercentage = 40,
            TargetDate = DateTime.UtcNow.AddMonths(6),
            Status = "Active"
        };

        _mockService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(goalResponse);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<DiversityGoalResponse>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
