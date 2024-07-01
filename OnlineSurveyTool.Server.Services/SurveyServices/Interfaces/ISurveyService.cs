using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

public interface ISurveyService
{
    Task<IResult<SurveyDTO>> AddSurvey(string ownerLogin, SurveyDTO surveyDto);
    Task<IResult<SurveyDTO>> EditSurvey(string ownerLogin, SurveyDTO editedSurvey);
    Task<IResult> DeleteSurvey(string surveyId);
    Task<IResult> OpenSurvey(string surveyId);
    Task<IResult> CloseSurvey(string surveyId);
}