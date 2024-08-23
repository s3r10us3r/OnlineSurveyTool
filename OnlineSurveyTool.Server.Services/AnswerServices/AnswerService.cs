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
    
    public AnswerService(IUnitOfWork uow, ISurveyResultValidator validator)
    {
        _surveyResultRepo = uow.SurveyResultRepo;
        _surveyRepo = uow.SurveyRepo;
        _validator = validator;
    }
    
    public async Task<IResult<SurveyResult, AddResultFailureReason>> AddResult(SurveyResultDTO result)
    {
        if (!await DoesSurveyExist(result.Id))
            return Result<SurveyResult, AddResultFailureReason>.Failure("Survey with this id does not exist!", 
                AddResultFailureReason.NonExistent);

        if (!await _validator.ValidateSurveyResultDto(result))
            return Result<SurveyResult, AddResultFailureReason>.Failure("Invalid data provided!",
                AddResultFailureReason.InvalidData);

        var surveyResult = DtoToResult(result);
        return Result<SurveyResult, AddResultFailureReason>.Success(surveyResult);
    }

    private async Task<bool> DoesSurveyExist(string id)
    {
        var survey = await _surveyRepo.GetOne(id);
        return survey is null;
    }
    
    private SurveyResult DtoToResult(SurveyResultDTO dto)
    {
        var result = new SurveyResult
        {
            SurveyId = dto.Id,
            TimeStamp = DateTime.Now,
            Answers = dto.Answers.Select(DtoToAnswer).ToList()
        };

        return result;
    }

    private Answer DtoToAnswer(AnswerDTO dto)
    {
        var answer = new Answer
        {
            
        };

        return answer;
    }
}