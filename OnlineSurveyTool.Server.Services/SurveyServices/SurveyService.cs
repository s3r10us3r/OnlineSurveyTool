using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices;

public class SurveyService: ISurveyService
{
    private readonly ISurveyRepo _surveyRepo;
    private readonly IConfiguration _config;
    private readonly ISurveyConverter _surveyConverter;
    private readonly ISurveyValidator _surveyValidator;
    private readonly ILogger<ISurveyService> _logger;

    public SurveyService(ISurveyRepo surveyRepo, ISurveyConverter surveyConverter, IConfiguration config,
        ISurveyValidator surveyValidator, ILogger<ISurveyService> logger)
    {
        _surveyRepo = surveyRepo;
        _surveyConverter = surveyConverter;
        _surveyValidator = surveyValidator;
        _config = config;
        _logger = logger;
    }

    public async Task<IResult<SurveyDTO>> AddSurvey(SurveyDTO surveyDto, User owner)
    {
        if (!_surveyValidator.ValidateSurvey(surveyDto, out var message))
        {
            _logger.LogWarning("Invalid surveyDTO supplied to AddSurvey() reason: {reason}", message);
            return Result<SurveyDTO>.Failure(message);
        }
        
        var survey = _surveyConverter.DtoToSurvey(surveyDto, owner);
        survey.ExternalId = await GenerateGuid();
        int res = await _surveyRepo.Add(survey);
        if (res == 0)
        {
            _logger.LogError("Can't add survey to database owner: {owner}", owner.Id);
            throw new DbUpdateException("Failed to add survey to the database");
        }
        _logger.LogInformation("Survey added to the database survey id: {surveyId} owner: {ownerId}", survey.Id, owner.Id);
        var newDto = _surveyConverter.SurveyToDto(survey);
        return Result<SurveyDTO>.Success(newDto);
    }
    
    public async Task<SurveyDTO?> GetSurvey(string surveyId)
    {
        var survey = await _surveyRepo.GetOne(surveyId);
        if (survey is null)
        {
            return null;
        }
        var dto = _surveyConverter.SurveyToDto(survey);
        return dto;
    }

    public async Task<SurveyDTO?> GetSurveyFromUser(string surveyId, User user)
    {
        var survey = user.Surveys.FirstOrDefault(s => s.ExternalId == surveyId);
        if (survey is null)
        {
            return null;
        }
        var surveyDto = _surveyConverter.SurveyToDto(survey);
        return surveyDto;
    }

    //TODO: FIX THIS SHIT
    public async Task<IResult<SurveyDTO, EditSurveyFailureReason>> EditSurvey(SurveyDTO surveyDto)
    {
        var survey = await _surveyRepo.GetOne(surveyDto.Id);
        if (survey is null)
        {
            _logger.LogWarning("User tried to edit an unexistant survey with id {surveyId}", surveyDto.Id);
            return Result<SurveyDTO, EditSurveyFailureReason>.Failure("Survey not found", EditSurveyFailureReason.NOT_FOUND);
        }

        var validated = _surveyValidator.ValidateSurvey(surveyDto, out var message);
        if (!validated)
        {
            return Result<SurveyDTO, EditSurveyFailureReason>.Failure(message, EditSurveyFailureReason.BAD_REQUEST);
        }
        
        var convertedDto = _surveyConverter.DtoToSurvey(surveyDto, survey.Owner);
        survey.Name = convertedDto.Name;
        survey.ClosingDate = convertedDto.ClosingDate;
        survey.OpeningDate = convertedDto.OpeningDate;

        foreach (var question in convertedDto.Questions)
        {
            if (question.ExternalId is null)
            {
                question.ExternalId = await GenerateGuid();
                
            }
        }

        return null;
    }

    private async Task<string> GenerateGuid()
    {
        var guid =  Guid.NewGuid().ToString();
        return guid;
    }

}