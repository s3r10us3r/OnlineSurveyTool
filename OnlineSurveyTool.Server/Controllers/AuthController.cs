using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Responses;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;

namespace OnlineSurveyTool.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IJWTokenService _jwTokenService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly CookieOptions _cookieOptions;

        public AuthController(IAuthenticationService authService, IUserService userService,
            IJWTokenService jwTokenService, ILogger<AuthController> logger, ICookieOptionsProvider cookieOptionsProvider)
        {
            _userService = userService;
            _authService = authService;
            _jwTokenService = jwTokenService;
            _logger = logger;
            _cookieOptions = cookieOptionsProvider.GetCookieOptions();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Register() {model}", userDTO);
                return BadRequest(new {Message = "Invalid data provided", errors = ModelState});
            }
            try
            {
                var result = await _userService.CreateUser(userDTO);
                if (result.IsSuccess)
                {
                    var user = result.Value;
                    return CreatedAtAction(nameof(Register), user);
                }
                if (result.Reason == UserCreationFailure.NameConflict)
                {
                    return Conflict(new { Message = "User with this login already exists!" });
                }
                return BadRequest(new { Message = "Invalid credentials have been supplied!" });
            } catch (Exception e)
            {
                _logger.LogError("Error occured in AuthController.Register {e}", e);
                return StatusCode(500, new { Message = "Internal server error!" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to AuthController.Login() {model}", loginDTO);
                return BadRequest(new { Message = "Invalid data provided", errors = ModelState });
            }
            try
            {
                var result = await _authService.AuthenticateUser(loginDTO);
                if (!result.IsSuccess)
                {
                    _logger.LogInformation("Invalid credentials have been supplied message: {message}", result.Message);
                    return Unauthorized(new { Message = "Invalid credentials!" });
                }
                var user = result.Value;

                string accessToken = _jwTokenService.GenerateAccessToken(user, out var accessExpiration);
                string refreshToken = _jwTokenService.GenerateRefreshToken(user, out var refreshExpiration);

                Response.Cookies.Append("refresh-token", refreshToken, _cookieOptions);
                
                return Ok(new LoginResponse
                {
                    AccessToken = accessToken, AccessTokenExpirationDateTime = accessExpiration,
                });
            } catch (Exception e)
            {
                _logger.LogError("Error occured in AuthController.Login {e}", e);
                return StatusCode(500, new { Message = "Internal server error!" });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh-token"];
            if (refreshToken is null)
            {
                return Unauthorized(new {ErrorMessage = "The 'refresh-token' cookie has not been set!"});
            }
            var claimsPrincipalResult = _jwTokenService.GetClaimsPrincipalFromRefreshToken(refreshToken);
            if (!claimsPrincipalResult.IsSuccess)
            {
                var errorMessage = claimsPrincipalResult.Reason == RefreshFailure.SecurityTokenExpired
                    ? "Refresh token has expired."
                    : "Invalid refresh token.";
                return Unauthorized(new {ErrorMessage = errorMessage});
            }

            try
            {
                var user = await _userService.GetUserFromClaimsPrincipal(claimsPrincipalResult.Value);
                var accessToken = _jwTokenService.GenerateAccessToken(user!, out var expiration);
                return Ok(new { AccessToken = accessToken, AccessTokenExpirationDateTime = expiration });
            }
            catch (ArgumentException e)
            {
                _logger.LogWarning("Invalid refresh token supplied to Refresh {e}", e);
                return Unauthorized(new { ErrorMessage = "Invalid refresh token." });
            }
        }
    }
}
