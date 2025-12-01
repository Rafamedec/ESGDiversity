using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InclusionEventsController : ControllerBase
    {
        private readonly IInclusionEventService _service;
        private readonly ILogger<InclusionEventsController> _logger;

        public InclusionEventsController(
            IInclusionEventService service,
            ILogger<InclusionEventsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os eventos de inclusão
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<InclusionEventSummaryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<InclusionEventSummaryViewModel>>> GetInclusionEvents(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? category = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            _logger.LogInformation(
                "Fetching inclusion events - Page: {Page}, PageSize: {PageSize}",
                page, pageSize);

            var result = await _service.GetInclusionEventsAsync(page, pageSize, category);
            return Ok(result);
        }

        /// <summary>
        /// Obtém um evento por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InclusionEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InclusionEventResponse>> GetById(int id)
        {
            var evt = await _service.GetByIdAsync(id);

            if (evt == null)
            {
                return NotFound(new { message = "Inclusion event not found" });
            }

            return Ok(evt);
        }

        /// <summary>
        /// Cria um novo evento de inclusão
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(typeof(InclusionEventResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<InclusionEventResponse>> Create(
            [FromBody] CreateInclusionEventRequest request)
        {
            _logger.LogInformation("Creating new inclusion event: {Title}", request.Title);

            var evt = await _service.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
        }

        /// <summary>
        /// Atualiza um evento de inclusão
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(typeof(InclusionEventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InclusionEventResponse>> Update(
            int id,
            [FromBody] UpdateInclusionEventRequest request)
        {
            var evt = await _service.UpdateAsync(id, request);

            if (evt == null)
            {
                return NotFound(new { message = "Inclusion event not found" });
            }

            return Ok(evt);
        }

        /// <summary>
        /// Deleta um evento de inclusão
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Inclusion event not found" });
            }

            return NoContent();
        }
    }
}
