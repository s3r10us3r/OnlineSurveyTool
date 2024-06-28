using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

public interface ISurveyService
{
    Task<IResult<SurveyDTO>> AddSurvey(SurveyDTO surveyDto, User owner);
    Task<SurveyDTO?> GetSurvey(string surveyId);
    Task<SurveyDTO?> GetSurveyFromUser(string surveyId, User user);
    Task<IResult<SurveyDTO, EditSurveyFailureReason>> EditSurvey(SurveyDTO surveyDto);
}

public enum EditSurveyFailureReason
{
    BAD_REQUEST, NOT_FOUND
}