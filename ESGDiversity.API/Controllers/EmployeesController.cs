using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService service, ILogger<EmployeesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os funcionários com paginação
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<EmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResponse<EmployeeResponse>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? department = null)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Invalid pagination parameters");
            }

            _logger.LogInformation(
                "Fetching employees - Page: {Page}, PageSize: {PageSize}, Department: {Department}",
                page, pageSize, department);

            var result = await _service.GetAllAsync(page, pageSize, department);
            return Ok(result);
        }

        /// <summary>
        /// Obtém um funcionário por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            _logger.LogInformation("Fetching employee with ID: {Id}", id);

            var employee = await _service.GetByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Employee not found with ID: {Id}", id);
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(employee);
        }

        /// <summary>
        /// Cria um novo funcionário
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
        {
            _logger.LogInformation("Creating new employee: {Name}", request.Name);

            var employee = await _service.CreateAsync(request);

            _logger.LogInformation("Employee created successfully with ID: {Id}", employee.Id);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Atualiza um funcionário existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeResponse>> Update(
            int id,
            [FromBody] UpdateEmployeeRequest request)
        {
            _logger.LogInformation("Updating employee with ID: {Id}", id);

            var employee = await _service.UpdateAsync(id, request);

            if (employee == null)
            {
                _logger.LogWarning("Employee not found with ID: {Id}", id);
                return NotFound(new { message = "Employee not found" });
            }

            _logger.LogInformation("Employee updated successfully with ID: {Id}", id);
            return Ok(employee);
        }

        /// <summary>
        /// Deleta um funcionário (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting employee with ID: {Id}", id);

            var success = await _service.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("Employee not found with ID: {Id}", id);
                return NotFound(new { message = "Employee not found" });
            }

            _logger.LogInformation("Employee deleted successfully with ID: {Id}", id);
            return NoContent();
        }
    }
}
