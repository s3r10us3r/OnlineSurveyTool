using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public interface IQuestionValidator
{
    public bool ValidateQuestions(List<QuestionDTO> questions, out string message);
    public bool ValidateQuestion(QuestionDTO question, out string message);

}