using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

public interface ISurveyService
{
    Task<SurveyDTO> AddSurvey(SurveyDTO surveyDto, User owner);
    Task<IResult<SurveyDTO>> GetSurvey(string surveyId);
}