using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

namespace OnlineSurveyTool.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IUserService _userService;
        private readonly ILogger<SurveyController> _logger;
        
        public SurveyController(ISurveyService surveyService, IUserService userService, ILogger<SurveyController> logger)
        {
            _surveyService = surveyService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("get/{surveyId}")]
        public async Task<IActionResult> Get(string surveyId)
        {
            var survey = await _surveyService.GetSurvey(surveyId);
            if (survey is null)
            {
                return NotFound(new {Message = "Survey not found!"});
            }
            if (survey.IsOpen)
            {
                _logger.LogInformation("Survey {id} requested", surveyId);
                return Ok(survey);
            }

            return NotFound(new { Message = "Survey not found!" });
        }

        [Authorize]
        [HttpGet("getAuth/{surveyId}")]
        public async Task<IActionResult> GetAuth(string surveyId)
        {
            var user = await _userService.GetUserFromClaimsPrincipal(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }

            var survey = await _surveyService.GetSurveyFromUser(surveyId, user);
            if (survey is null)
            {
                return Unauthorized();
            }

            return Ok(survey);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyDTO surveyDto)
        {
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad model supplied to CreateSurvey {model}", surveyDto);
                return BadRequest(new {Message = "Invalid data provided", errors = ModelState});
            }
            
            var user = await _userService.GetUserFromClaimsPrincipal(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }

            try
            {
                var res = await _surveyService.AddSurvey(surveyDto, user);
                if (res.IsSuccess)
                {
                    return CreatedAtAction(nameof(CreateSurvey), res.Value);
                }
                return BadRequest(new { Message = "Invalid data provided" });
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured in SurveyController.CreateSurvey {error}", e);
                return StatusCode(500, new { Message = "Internal server error!" });
            }
        }
    }
}