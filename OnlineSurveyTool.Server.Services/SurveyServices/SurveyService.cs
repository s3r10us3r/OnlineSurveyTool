using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices;

public class SurveyService : ISurveyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISurveyValidator _surveyValidator;
    private readonly ISurveyConverter _surveyConverter;
    private readonly IQuestionRepo _questionRepo;
    private readonly ISurveyRepo _surveyRepo;
    private readonly IUserRepo _userRepo;
    private readonly ILogger<SurveyService> _logger;

    public SurveyService(IUnitOfWork unitOfWork, ISurveyValidator surveyValidator, ISurveyConverter surveyConverter,
        ILogger<SurveyService> logger)
    {
        _unitOfWork = unitOfWork;
        _surveyValidator = surveyValidator;
        _questionRepo = _unitOfWork.QuestionRepo;
        _surveyRepo = _unitOfWork.SurveyRepo;
        _userRepo = _unitOfWork.UserRepo;
        _surveyConverter = surveyConverter;
        _logger = logger;
    }
    
    public async Task<IResult<SurveyDTO>> AddSurvey(string ownerLogin, SurveyDTO surveyDto)
    {
        if (!_surveyValidator.ValidateSurvey(surveyDto, out var message))
        {
            _logger.LogWarning("Invalid survey supplied to SurveyService.AddSurvey error message: {message}", message);
            return Result<SurveyDTO>.Failure(message);
        }

        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            _logger.LogError("Nonexistent user login {login} supplied to SurveyService.AddSurvey", ownerLogin);
            return Result<SurveyDTO>.Failure("User with this login does not exist.");
        }

        var survey = _surveyConverter.DtoToSurvey(surveyDto);
        survey.OwnerId = user.Id;
        var addResult = await _surveyRepo.Add(survey);
        if (addResult == 0)
        {
            _logger.LogError("Survey could not be added to the database in SurveyService.AddSurvey");
            throw new DbUpdateException("Could not add survey to the database.");
        }

        var resultDto = _surveyConverter.SurveyToDto(survey);
        return Result<SurveyDTO>.Success(resultDto);
    }

    public async Task<IResult<SurveyDTO>> EditSurvey(string ownerLogin, SurveyDTO editedSurvey)
    {
        if (editedSurvey.Id is null)
        {
            _logger.LogWarning("Survey supplied to surveyDto has no id.");
            return Result<SurveyDTO>.Failure("editedSurvey has no Id");
        }
        var survey = await _surveyRepo.GetOne(editedSurvey.Id);
        if (survey is null)
        {
            _logger.LogWarning("Survey with incorrect id supplied to SurveyService.EditSurvey id: {id}",
                editedSurvey.Id);
            return Result<SurveyDTO>.Failure("Survey with this id does not exist!");
        }

        var user = survey.Owner;
        if (user.Login != ownerLogin)
        {
            _logger.LogWarning("User {uL} supplied to EditSurvey is not the owner of survey {sId}", ownerLogin,
                survey.Id);
            return Result<SurveyDTO>.Failure("Survey with this id is owned by another user.");
        }
        
        
    }

    public async Task<IResult> DeleteSurvey(string surveyId)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> OpenSurvey(string surveyId)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> CloseSurvey(string surveyId)
    {
        throw new NotImplementedException();
    }

}