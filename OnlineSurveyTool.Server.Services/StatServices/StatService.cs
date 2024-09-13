using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices;

public class StatService : IStatService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepo _userRepo;
    private readonly ISurveyRepo _surveyRepo;
    private readonly ISurveyResultRepo _surveyResultRepo;
    private readonly IQuestionRepo _questionRepo;
    private readonly IAnswerStatsHelper _answerStatsHelper;
    
    public StatService(IUnitOfWork uow, IAnswerStatsHelper answerStatsHelper)
    {
        _uow = uow;
        _userRepo = _uow.UserRepo;
        _surveyRepo = _uow.SurveyRepo;
        _questionRepo = _uow.QuestionRepo;
        _surveyResultRepo = _uow.SurveyResultRepo;
        _answerStatsHelper = answerStatsHelper;
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
    
    public async Task<SurveyResultsStatistics?> GetSurveyStats(string surveyId, User user)
    {
        var survey = await _surveyRepo.GetOne(surveyId);
        if (survey is null || survey.OwnerId != user.Id)
            return null;
        await _surveyRepo.LoadResults(survey);
        return new SurveyResultsStatistics(survey.Name, survey.Results.Count, await GetListOfQuestionStats(survey));
    }

    private async Task<ICollection<AnswerStatistics>> GetListOfQuestionStats(Survey survey)
    {
        var answerStats = new List<AnswerStatistics>();
        await _surveyRepo.LoadResults(survey);
        await LoadAnswers(survey.Results);
        foreach (var q in survey.Questions)
            answerStats.Add(await _answerStatsHelper.GetAnswerStats(q, survey.Results));
        return answerStats;
    }
    
    public async Task<AnswerStatistics?> GetQuestionStats(string questionId, User user)
    {
        var question = await _questionRepo.GetOne(questionId);
        if (question == null || question.Survey.OwnerId != user.Id)
            return null;

        var survey = question.Survey;
        await _surveyRepo.LoadResults(survey);
        await LoadAnswers(survey.Results);
        return await _answerStatsHelper.GetAnswerStats(question, survey.Results);
    }

    private async Task LoadAnswers(IEnumerable<SurveyResult> results)
    {
        foreach (var result in results)
            await _surveyResultRepo.LoadAnswers(result);
    }
}
