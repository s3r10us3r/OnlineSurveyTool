using OnlineSurveyTool.Server.Services.StatServices.DTOs;

namespace OnlineSurveyTool.Server.Services.StatServices.Interfaces;

public interface IStatService
{
    Task<IEnumerable<SurveyHeaderDto>> GetSurveyHeadersForUser(string login);
}