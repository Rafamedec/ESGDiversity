using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class DiversityMetricsControllerTests
{
    private readonly Mock<IDiversityMetricsService> _mockService;
    private readonly Mock<ILogger<DiversityMetricsController>> _mockLogger;
    private readonly DiversityMetricsController _controller;

    public DiversityMetricsControllerTests()
    {
        _mockService = new Mock<IDiversityMetricsService>();
        _mockLogger = new Mock<ILogger<DiversityMetricsController>>();
        _controller = new DiversityMetricsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetDiversityMetrics_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<DiversityMetricsViewModel>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 50,
            TotalPages = 5,
            Data = new List<DiversityMetricsViewModel>()
        };

        _mockService
            .Setup(s => s.GetDiversityMetricsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetDiversityMetrics(1, 10, null);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<DiversityMetricsViewModel>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
