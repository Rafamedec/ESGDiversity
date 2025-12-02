using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class SalaryEquityControllerTests
{
    private readonly Mock<ISalaryEquityService> _mockService;
    private readonly Mock<ILogger<SalaryEquityController>> _mockLogger;
    private readonly SalaryEquityController _controller;

    public SalaryEquityControllerTests()
    {
        _mockService = new Mock<ISalaryEquityService>();
        _mockLogger = new Mock<ILogger<SalaryEquityController>>();
        _controller = new SalaryEquityController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSalaryEquityAnalysis_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<SalaryEquityViewModel>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 15,
            TotalPages = 2,
            Data = new List<SalaryEquityViewModel>()
        };

        _mockService
            .Setup(s => s.GetSalaryEquityAnalysisAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetSalaryEquityAnalysis(1, 10, null);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<SalaryEquityViewModel>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
