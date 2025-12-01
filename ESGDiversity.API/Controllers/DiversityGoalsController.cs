using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,HR")]
    public class DiversityGoalsController : ControllerBase
    {
        private readonly IDiversityGoalService _service;
        private readonly ILogger<DiversityGoalsController> _logger;

        public DiversityGoalsController(
            IDiversityGoalService service,
            ILogger<DiversityGoalsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Lista todas as metas de diversidade
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<DiversityGoalResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResponse<DiversityGoalResponse>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            var result = await _service.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Obtém uma meta por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DiversityGoalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DiversityGoalResponse>> GetById(int id)
        {
            var goal = await _service.GetByIdAsync(id);

            if (goal == null)
            {
                return NotFound(new { message = "Diversity goal not found" });
            }

            return Ok(goal);
        }

        /// <summary>
        /// Cria uma nova meta de diversidade
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DiversityGoalResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<DiversityGoalResponse>> Create(
            [FromBody] CreateDiversityGoalRequest request)
        {
            _logger.LogInformation("Creating new diversity goal for department: {Department}", request.Department);

            var goal = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
        }

        /// <summary>
        /// Atualiza uma meta de diversidade
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DiversityGoalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DiversityGoalResponse>> Update(
            int id,
            [FromBody] UpdateDiversityGoalRequest request)
        {
            var goal = await _service.UpdateAsync(id, request);

            if (goal == null)
            {
                return NotFound(new { message = "Diversity goal not found" });
            }

            return Ok(goal);
        }

        /// <summary>
        /// Deleta uma meta de diversidade
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Diversity goal not found" });
            }

            return NoContent();
        }
    }
}
