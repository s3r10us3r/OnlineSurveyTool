using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Requests;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.StatServices;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;

namespace OnlineSurveyTool.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly IStatService _statService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    
    public StatsController(IStatService statService, IUserService userService, ILogger<StatService> logger)
    {
        _statService = statService;
        _userService = userService;
        _logger = logger;
    }
    
    [Authorize]
    [HttpGet("getSurveyHeaders")]
    public async Task<IActionResult> GetSurveyHeaders()
    {
        try
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                _logger.LogWarning("Bad identity supplied to SurveyController.GetSurveyHeaders()");
                return Unauthorized();
            }

            var userLogin = identity.FindFirst("sub")?.Value;

            if (userLogin == null)
            {
                _logger.LogWarning("User login not found in token");
                return Unauthorized(new { Message = "User not authorized." });
            }

            var headers = await _statService.GetSurveyHeadersForUser(userLogin);
            return Ok(headers);
        }
        catch (Exception e)
        {
            _logger.LogError("Error {e} in StatsController.GetSurveyHeaders", e);
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [Authorize]
    [HttpPost("surveyStats")]
    public async Task<IActionResult> GetSurveyStats([FromBody] StatsRequest request)
    {
        string surveyId = request.Id;
        try
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                _logger.LogWarning("Bad identity supplied to SurveyController.GetSurveyStats()");
                return Unauthorized();
            }

            var user = await _userService.GetUserFromIdentityClaims(identity);
            if (user is null)
            {
                _logger.LogWarning("Non existing user supplied to StatsController.GetSurveyStats()");
                return Unauthorized();
            }

            var stats = await _statService.GetSurveyStats(surveyId, user);
            if (stats is null)
                return NotFound();
            return Ok(stats);
        }
        catch (Exception e)
        {
            _logger.LogError("Error {e} in StatsController.GetSurveyStats", e);
            return StatusCode(500, new { Message = "Internal server error" });  
        }
    }
}