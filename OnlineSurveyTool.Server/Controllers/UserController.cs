using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.DTOs;
using OnlineSurveyTool.Server.Services.DTOs;
using OnlineSurveyTool.Server.Services.Interfaces;

namespace OnlineSurveyTool.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthenticationService authService, IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Register() {model}", userDTO);
                return BadRequest(new {message = "Invalid data provided", errors = ModelState});
            }

            try
            {
                var result = await _userService.CreateUser(userDTO);
                if (result.IsSuccess)
                {
                    var user = result.Value;
                    return CreatedAtAction(nameof(Register), user);
                }
                if (result.Message == "User with this login already exists!")
                {
                    return Conflict(new { message = result.Message });
                }
                return BadRequest(new { message = result.Message });
            } catch (Exception e)
            {
                _logger.LogError("Error occured in AuthController.Register {e}", e);
                return StatusCode(500, new { message = "Internal server error!" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Login() {model}", loginDTO);
                return BadRequest(new { message = "Invalid data provided", errors = ModelState });
            }

            try
            {
                var result = await _authService.AuthenticateUser(loginDTO);
                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message });
                }
                string token = result.Value;
                return Ok(new JWTResponseDTO { Token = token, TokenType = "bearer"});
            } catch(Exception e)
            {
                _logger.LogError("Error occured in AuthController.Login {e}", e);
                return StatusCode(500, new { message = "Internal server error!" });
            }
        }
    }
}
