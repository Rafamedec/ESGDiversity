using Microsoft.AspNetCore.Mvc;
using ESGDiversity.API.Services;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login e retorna token JWT
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for user: {Username}", request.Username);

            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            _logger.LogInformation("Successful login for user: {Username}", request.Username);
            return Ok(response);
        }

        /// <summary>
        /// Registra novo usuário e retorna token JWT
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", request.Username);

            var response = await _authService.RegisterAsync(request);

            if (response == null)
            {
                _logger.LogWarning("Failed registration - user already exists: {Username}", request.Username);
                return BadRequest(new { message = "Username or email already exists" });
            }

            _logger.LogInformation("Successful registration for user: {Username}", request.Username);
            return CreatedAtAction(nameof(Register), response);
        }
    }
}
