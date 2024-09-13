using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;

namespace OnlineSurveyTool.Server.Services.StatServices.Interfaces;

public interface IStatService
{
    Task<IEnumerable<SurveyHeaderDto>> GetSurveyHeadersForUser(string login);
    Task<AnswerStatistics?> GetQuestionStats(string questionId, User user);
    Task<SurveyResultsStatistics?> GetSurveyStats(string surveyId, User user);
}