using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class SurveyValidator : ISurveyValidator
{
    private readonly IQuestionValidator _questionValidator;
    
    public SurveyValidator(IQuestionValidator questionValidator)
    {
        _questionValidator = questionValidator;
    }
    
    public bool ValidateSurvey(SurveyDTO survey, out string message)
    {
        return _questionValidator.ValidateQuestions(survey.Questions, out message);
    }
}