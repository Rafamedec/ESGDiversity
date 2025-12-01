using Microsoft.AspNetCore.Mvc;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalProgressController : ControllerBase
    {
        private readonly IGoalProgressService _service;
        private readonly ILogger<GoalProgressController> _logger;

        public GoalProgressController(
            IGoalProgressService service,
            ILogger<GoalProgressController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<GoalProgressViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<GoalProgressViewModel>>> GetGoalProgress(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            _logger.LogInformation(
                "Fetching goal progress - Page: {Page}, PageSize: {PageSize}",
                page, pageSize);

            var result = await _service.GetGoalProgressAsync(page, pageSize);
            return Ok(result);
        }
    }
}
