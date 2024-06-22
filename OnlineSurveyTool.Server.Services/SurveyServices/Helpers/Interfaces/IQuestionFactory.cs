using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

public interface IQuestionFactory
{
    QuestionBase MakeQuestion(Question question);
}