using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public interface ISurveyValidator
{
    public bool ValidateSurvey(SurveyDTO survey, out string message);
}