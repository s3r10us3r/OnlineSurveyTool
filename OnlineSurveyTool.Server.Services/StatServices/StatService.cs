using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices;

public class StatService : IStatService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepo _userRepo;
    private readonly ISurveyRepo _surveyRepo;
    
    public StatService(IUnitOfWork uow)
    {
        _uow = uow;
        _userRepo = _uow.UserRepo;
        _surveyRepo = _uow.SurveyRepo;
    }
    
    public async Task<IEnumerable<SurveyHeaderDto>> GetSurveyHeadersForUser(string login)
    {
        var user = await _userRepo.GetOne(login);
        if (user is null)
            throw new ArgumentException("User with this login does not exist");
        await _userRepo.LoadSurveys(user);

        var headers = await ConvertToHeaders(user.Surveys);
        return headers;
    }

    private async Task<IEnumerable<SurveyHeaderDto>> ConvertToHeaders(IEnumerable<Survey> surveys)
    {
        var headers = new List<SurveyHeaderDto>();
        
        foreach (var survey in surveys)
            headers.Add(await ConvertToHeader(survey));
        
        return headers;
    }

    private async Task<SurveyHeaderDto> ConvertToHeader(Survey survey)
    {
        await _surveyRepo.LoadResults(survey);
        
        return new SurveyHeaderDto()
        {
            Id = survey.Id,
            IsOpen = survey.IsOpen,
            Name = survey.Name,
            ResultCount = survey.Results.Count
        };
    }
}