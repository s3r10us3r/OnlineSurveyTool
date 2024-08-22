using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

namespace OnlineSurveyTool.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(ISurveyService surveyService, ILogger<SurveyController> logger)
    {
        _surveyService = surveyService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] SurveyDTO dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Bad model supplied to SurveyController.Add() {model}", ModelState);
            return BadRequest(new {Message = "Invalid data provided!", errors = ModelState});
        }

        try
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                _logger.LogWarning("Bad identity supplied to SurveyController.Add()");
                return Unauthorized();
            }
            var userLogin = identity.FindFirst("sub")?.Value;
            if (userLogin is null)
            {
                _logger.LogWarning("Identity without name identifier supplied to SurveyController.Add()");
                return Unauthorized();
            }
            
            var result = await _surveyService.AddSurvey(userLogin, dto);
            if (result.IsFailure)
            {
                _logger.LogWarning("Adding survey failed {error}", result.Message);
                return BadRequest(new { Message = $"Failed to add survey to the database, error: {result.Message}" });
            }

            var surveyResult = result.Value; 
            _logger.LogInformation("User {userLogin} added survey {surveyId}", userLogin, surveyResult.Id);
            return CreatedAtAction(nameof(Add), surveyResult);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while adding survey {error}", e);
            return StatusCode(500, new { Message = "Internal server error!" } );
        }
    }

    [Authorize]
    [HttpPatch("edit")]
    public async Task<IActionResult> Edit([FromBody] SurveyEditDto surveyEditDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Bad model supplied to SurveyController.Edit() {model}", ModelState);
            return BadRequest(new {Message = "Invalid data provided!", errors = ModelState});
        }

        try
        {
            var userLogin = HttpContext.User.Identity?.Name;

            if (userLogin == null)
            {
                _logger.LogWarning("User login not found in token");
                return Unauthorized(new { Message = "User not authorized." });
            }

            var result = await _surveyService.EditSurvey(userLogin, surveyEditDto);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Survey edited successfully!", EditedSurvey = result });
            }

            return result.Reason switch
            {
                SurveyServiceFailureReason.DoesNotExist => NotFound(new { Message = "Survey has not been found." }),
                SurveyServiceFailureReason.NotAuthorized => Unauthorized(new { Message = "User does not have access to this resource" }),
                SurveyServiceFailureReason.InvalidRequest => BadRequest(new { Message = "Bad request.", Error = result.Message }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured in SurveyController.Edit() error: {e}", e);
            return StatusCode(500, new { Message = "Internal server error!" });
        }
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var userLogin = HttpContext.User.Identity?.Name;
            if (userLogin == null)
            {
                _logger.LogWarning("User login not found in token.");
                return Unauthorized(new { Message = "User not authorized." });
            }

            var result = await _surveyService.DeleteSurvey(userLogin, id);
            if (result.IsFailure)
            {
                return NotFound(new { Message = "User does not own a survey with this id" });
            }

            return Ok(new { Message = $"Survey with id {id} deleted successfully" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while deleting a survey");
            return StatusCode(500, new { Message = "internal server error!" });
        }
    }

    [Authorize]
    [HttpPatch("close/{id}")]
    public async Task<IActionResult> Close(string id)
    {
        try
        {
            var userLogin = HttpContext.User.Identity?.Name;
            if (userLogin == null)
            {
                _logger.LogWarning("User login not found in token.");
                return Unauthorized(new { Message = "User not authorized." });
            }

            var result = await _surveyService.CloseSurvey(userLogin, id);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Survey closed successfully" });
            }

            return NotFound(new { Message = "User does not have access to a survey with this id." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while closing survey");
            return StatusCode(500, "Internal server error!");
        }
    }

    [Authorize]
    [HttpPatch("open/{id}")]
    public async Task<IActionResult> Open(string id)
    {
        try
        {
            var userLogin = HttpContext.User.Identity?.Name;
            if (userLogin == null)
            {
                _logger.LogWarning("User login not found in token.");
                return Unauthorized(new { Message = "User not authorized." });
            }

            var result = await _surveyService.OpenSurvey(userLogin, id);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Survey opened successfully" });
            }

            return NotFound(new { Message = "User does not have access to a survey with this id." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while opening survey");
            return StatusCode(500, "Internal server error!");
        }
    }


    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetSurvey(string id)
    {
        try
        {
            var result = await _surveyService.GetSurvey(id);
            if (result.IsSuccess)
            {
                var survey = result.Value;
                return Ok(survey);
            }

            return NotFound("Survey not found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while getting survey");
            return StatusCode(500, new {Message = "Internal server error"});
        }
    }
}
