using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.StatServices;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;

namespace OnlineSurveyTool.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly IStatService _statService;
    private readonly ILogger _logger;
    
    public StatsController(IStatService statService, ILogger<StatService> logger)
    {
        _statService = statService;
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
                _logger.LogWarning("Bad identity supplied to SurveyController.Edit()");
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
}