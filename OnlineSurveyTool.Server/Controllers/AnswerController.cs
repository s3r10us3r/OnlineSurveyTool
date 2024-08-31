using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Requests;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.AnswerServices.Interfaces;
using IResult = OnlineSurveyTool.Server.Services.Utils.Interfaces.IResult;

namespace OnlineSurveyTool.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;
    private readonly ILogger<AnswerController> _logger;
    
    public AnswerController(IAnswerService answerService, ILogger<AnswerController> logger)
    {
        _answerService = answerService;
        _logger = logger;
    }

    [HttpPost("answer")]
    public async Task<IActionResult> Answer([FromBody] SurveyResultDTO surveyResult)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model supplied to AnswerController.Answer()");
            return BadRequest(new { Message = "Invalid data provided!", errors = ModelState });
        }

        try
        {
            var result = await _answerService.AddResult(surveyResult);
            if (result.IsSuccess)
                return Ok();
            
            _logger.LogWarning("Adding survey failed with for reason: {reason}", result.Reason);
                
            return result.Reason switch
            {
                AddResultFailureReason.InvalidData => HandleInvalidData(result.Message),
                AddResultFailureReason.NonExistent => NotFound(
                    new { Message = $"Survey with id {surveyResult.Id} does not exist" }),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error while adding answer {error}", e);
            return StatusCode(500, new { Message = "Internal server error." });
        }
    }

    private BadRequestObjectResult HandleInvalidData(string message)
    {
        _logger.LogWarning("Invalid data provided to AnswerController.Answer message: {mess}", message);
        return BadRequest(new { Message = message });
    }
}
