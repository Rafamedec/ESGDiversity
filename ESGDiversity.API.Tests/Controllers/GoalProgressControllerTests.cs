using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class GoalProgressControllerTests
{
    private readonly Mock<IGoalProgressService> _mockService;
    private readonly Mock<ILogger<GoalProgressController>> _mockLogger;
    private readonly GoalProgressController _controller;

    public GoalProgressControllerTests()
    {
        _mockService = new Mock<IGoalProgressService>();
        _mockLogger = new Mock<ILogger<GoalProgressController>>();
        _controller = new GoalProgressController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetGoalProgress_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<GoalProgressViewModel>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 30,
            TotalPages = 3,
            Data = new List<GoalProgressViewModel>()
        };

        _mockService
            .Setup(s => s.GetGoalProgressAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetGoalProgress(1, 10);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<GoalProgressViewModel>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
