using System.Security.Cryptography;
using System.Text;
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
        if (!_surveyValidator.ValidateSurvey(surveyDto, out string message))
        {
            _logger.LogWarning("Invalid surveyDTO supplied to AddSurvey() reason: {reason}", message);
            return Result<SurveyDTO>.Failure(message);
        }
        
        var survey = _surveyConverter.DtoToSurvey(surveyDto, owner);
        survey.Token = await GenerateSurveyId();
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
    
    private static readonly char[] Chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
    
    private async Task<string> GenerateSurveyId()
    {
        while (true)
        {
            int size = int.Parse(_config["Settings:SurveyIDLength"]!);
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % Chars.Length;

                result.Append(Chars[idx]);
            }

            var id = result.ToString();
            if (await _surveyRepo.GetOne(id) is null)
            {
                return result.ToString();
            }
        }
    }
    
    
}