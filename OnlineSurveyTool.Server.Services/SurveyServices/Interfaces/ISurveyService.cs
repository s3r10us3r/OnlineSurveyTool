using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

public interface ISurveyService
{
    Task<IResult<SurveyDTO>> AddSurvey(string ownerLogin, SurveyDTO surveyDto);
    Task<IResult<SurveyDTO, EditSurveyFailureReason>> EditSurvey(string ownerLogin, SurveyEditDto editedSurvey);
    Task<IResult> DeleteSurvey(string ownerLogin, string surveyId);
    Task<IResult> OpenSurvey(string ownerLogin, string surveyId);
    Task<IResult> CloseSurvey(string ownerLogin, string surveyId);
}

public enum EditSurveyFailureReason
{
    DoesNotExist, NotAuthorized, InvalidRequest, ServerError
}