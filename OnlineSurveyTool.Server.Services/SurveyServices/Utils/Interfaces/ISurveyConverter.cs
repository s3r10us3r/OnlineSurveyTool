using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public interface ISurveyConverter
{
    Survey DtoToSurvey(SurveyDTO surveyDto);
    SurveyDTO SurveyToDto(Survey survey);
}