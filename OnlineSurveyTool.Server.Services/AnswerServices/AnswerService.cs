using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.AnswerServices.Interfaces;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices;

public class AnswerService : IAnswerService
{
    private readonly ISurveyResultRepo _surveyResultRepo;
    private readonly ISurveyRepo _surveyRepo;
    private readonly ISurveyResultValidator _validator;
    private readonly ISurveyResultConverter _converter;
    
    public AnswerService(IUnitOfWork uow, ISurveyResultValidator validator, ISurveyResultConverter converter)
    {
        _surveyResultRepo = uow.SurveyResultRepo;
        _surveyRepo = uow.SurveyRepo;
        _validator = validator;
        _converter = converter;
    }
    
    public async Task<IResult<SurveyResult, AddResultFailureReason>> AddResult(SurveyResultDTO result)
    {
        if (!await DoesSurveyExist(result.Id))
            return Result<SurveyResult, AddResultFailureReason>.Failure("Survey with this id does not exist!", 
                AddResultFailureReason.NonExistent);

        var validationResult = await _validator.ValidateSurveyResultDto(result);

        if (validationResult.IsFailure)
        {
            return Result<SurveyResult, AddResultFailureReason>.Failure(validationResult.Message,
                AddResultFailureReason.InvalidData);
        }

        var surveyResult = await DtoToResult(result);
        int res = await _surveyResultRepo.Add(surveyResult);
        
        if (res > 0)
            return Result<SurveyResult, AddResultFailureReason>.Success(surveyResult);
        
        return Result<SurveyResult, AddResultFailureReason>.Failure("Could not add result to the database.",
            AddResultFailureReason.CouldNotAddToDb);
    }

    private async Task<bool> DoesSurveyExist(string id)
    {
        var survey = await _surveyRepo.GetOne(id);
        return survey is not null;
    }
    
    private async Task<SurveyResult> DtoToResult(SurveyResultDTO dto)
    {
        var result = await _converter.SurveyResultToModel(dto);
        return result;
    }
}
