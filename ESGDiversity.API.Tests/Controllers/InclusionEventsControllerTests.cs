using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESGDiversity.API.Controllers;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Tests.Controllers;

public class InclusionEventsControllerTests
{
    private readonly Mock<IInclusionEventService> _mockService;
    private readonly Mock<ILogger<InclusionEventsController>> _mockLogger;
    private readonly InclusionEventsController _controller;

    public InclusionEventsControllerTests()
    {
        _mockService = new Mock<IInclusionEventService>();
        _mockLogger = new Mock<ILogger<InclusionEventsController>>();
        _controller = new InclusionEventsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetInclusionEvents_ReturnsOk_WhenParametersAreValid()
    {
        // Arrange
        var paginatedResponse = new PaginatedResponse<InclusionEventSummaryViewModel>
        {
            Page = 1,
            PageSize = 10,
            TotalCount = 25,
            TotalPages = 3,
            Data = new List<InclusionEventSummaryViewModel>()
        };

        _mockService
            .Setup(s => s.GetInclusionEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(paginatedResponse);

        // Act
        var result = await _controller.GetInclusionEvents(1, 10, null);

        // Assert
        var okResult = Assert.IsType<ActionResult<PaginatedResponse<InclusionEventSummaryViewModel>>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEventExists()
    {
        // Arrange
        var eventResponse = new InclusionEventResponse
        {
            Id = 1,
            Title = "Diversity Workshop",
            Description = "Test description",
            EventDate = DateTime.UtcNow.AddDays(10),
            Category = "Training",
            ParticipantsCount = 85,
            Budget = 5000,
            Department = "HR",
            Status = "Scheduled"
        };

        _mockService
            .Setup(s => s.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(eventResponse);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<ActionResult<InclusionEventResponse>>(result);
        var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
        Assert.Equal(200, objectResult.StatusCode);
    }
}
