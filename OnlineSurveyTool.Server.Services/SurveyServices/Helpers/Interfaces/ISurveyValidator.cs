using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

public interface ISurveyValidator
{
    bool ValidateSurvey(SurveyDTO surveyDto, out string message);
    bool ValidateQuestion(QuestionBase questionBase, out string message);
}