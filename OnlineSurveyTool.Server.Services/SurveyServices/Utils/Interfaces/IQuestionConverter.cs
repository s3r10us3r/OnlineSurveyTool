using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public interface IQuestionConverter
{
    Question DtoToQuestion(QuestionDTO dto);
    QuestionDTO QuestionToDto(Question question);
}