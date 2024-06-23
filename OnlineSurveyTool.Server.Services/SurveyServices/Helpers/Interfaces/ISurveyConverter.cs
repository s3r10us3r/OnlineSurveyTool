using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

public interface ISurveyConverter
{
    SurveyDTO SurveyToDto(Survey survey);
    Survey DtoToSurvey(SurveyDTO dto, User owner);
}