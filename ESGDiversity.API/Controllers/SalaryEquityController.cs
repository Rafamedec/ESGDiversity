using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,HR")]
    public class SalaryEquityController : ControllerBase
    {
        private readonly ISalaryEquityService _service;
        private readonly ILogger<SalaryEquityController> _logger;

        public SalaryEquityController(
            ISalaryEquityService service,
            ILogger<SalaryEquityController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<SalaryEquityViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PaginatedResponse<SalaryEquityViewModel>>> GetSalaryEquityAnalysis(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? department = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            _logger.LogInformation(
                "Fetching salary equity analysis - Page: {Page}, PageSize: {PageSize}",
                page, pageSize);

            var result = await _service.GetSalaryEquityAnalysisAsync(page, pageSize, department);
            return Ok(result);
        }
    }
}
