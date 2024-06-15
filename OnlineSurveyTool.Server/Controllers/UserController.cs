using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.DTOs;
using OnlineSurveyTool.Server.Services.Interfaces;

namespace OnlineSurveyTool.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthenticationService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Register() {model}", userDTO);
                return BadRequest(new {message = "Invalid data provided", errors = ModelState});
            }

            try
            {
                var result = _authService.CreateUser(userDTO.Login, userDTO.EMail, userDTO.Password);
                if (result.IsSuccess)
                {
                    User user = result.Value;
                    return CreatedAtAction(nameof(Register), new UserDTO { Login = user.Login, EMail = user.EMail });
                }
                return BadRequest(new { message = result.Message });
            } catch (Exception e)
            {
                _logger.LogError("Error occured in AuthController.Register {e}", e);
                return StatusCode(500, new { message = "Internal server error!" });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Login() {model}", loginDTO);
                return BadRequest(new { message = "Invalid data provided", errors = ModelState });
            }

            try
            {
                var result = _authService.AuthenticateUser(loginDTO.Login, loginDTO.Password);
                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message });
                }
                string token = result.Value;
                return Ok(new { token });
            } catch(Exception e)
            {
                _logger.LogError("Error occured in AuthController.Login {e}", e);
                return StatusCode(500, new { message = "Internal server error!" });
            }
        }
    }
}
