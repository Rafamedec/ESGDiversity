using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiversityMetricsController : ControllerBase
    {
        private readonly IDiversityMetricsService _service;
        private readonly ILogger<DiversityMetricsController> _logger;

        public DiversityMetricsController(
            IDiversityMetricsService service,
            ILogger<DiversityMetricsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<DiversityMetricsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<DiversityMetricsViewModel>>> GetDiversityMetrics(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? department = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            _logger.LogInformation(
                "Fetching diversity metrics - Page: {Page}, PageSize: {PageSize}, Department: {Department}",
                page, pageSize, department);

            var result = await _service.GetDiversityMetricsAsync(page, pageSize, department);
            return Ok(result);
        }
    }
}
