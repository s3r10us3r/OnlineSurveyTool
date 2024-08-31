using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

public interface ISurveyResultValidator
{
    Task<IResult> ValidateSurveyResultDto(SurveyResultDTO dto);
}