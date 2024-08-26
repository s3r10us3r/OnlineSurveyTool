using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Interfaces;

public interface IAnswerService
{
    public Task<IResult<SurveyResult, AddResultFailureReason>> AddResult(SurveyResultDTO result);
}

public enum AddResultFailureReason
{
    InvalidData, NonExistent, SurveyClosed, CouldNotAddToDb
}
