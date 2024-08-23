using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

public interface ISurveyResultValidator
{
    Task<bool> ValidateSurveyResultDto(SurveyResultDTO dto);
}