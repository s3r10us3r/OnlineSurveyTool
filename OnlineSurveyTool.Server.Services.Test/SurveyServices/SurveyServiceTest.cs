using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices;

[TestFixture]
public class SurveyServiceTest
{
    private ISurveyService _surveyService;
    private ISurveyRepo _surveyRepo;

    [SetUp]
    public void SetUp()
    {
        _surveyRepo = new SurveyRepoMock();
        _surveyService = new SurveyService(_surveyRepo,
            new SurveyConverter(new QuestionFactory(), new QuestionMapper()), ConfigurationCreator.MockConfig(),
            new SurveyValidator(), new LoggerMock<ISurveyService>());
    }

    [Test]
    public async Task AddSurveyShouldFailWithInvalidSurvey()
    {
        var survey = new SurveyDTO(){Questions = [], OpeningDate = DateTime.MaxValue, ClosingDate = DateTime.MinValue};
        var result = await _surveyService.AddSurvey(survey, new User());
        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task AddSurveyShouldSucceedWithValidSurvey()
    {
        var survey = new SurveyDTO()
        {
            OpeningDate = DateTime.Today,
            ClosingDate = DateTime.Today.AddDays(1),
            IsOpen = true,
            Name = "Survey",
            Questions = 
            [
                new SingleChoiceQuestionDTO()
                {
                    CanBeSkipped = false,
                    ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 2, Value = "Choice 2"}],
                    Number = 1,
                    Value = "question 1"
                },
                new MultipleChoiceQuestionDTO()
                {
                    CanBeSkipped = false,
                    ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 2, Value = "Choice 2"}],
                    Number = 2,
                    Value = "question 2",
                    MinimalChoices = 1,
                    MaximalChoices = 2
                },
                new NumericalDoubleQuestionDTO()
                {
                    CanBeSkipped = false,
                    MaximalAnswer = 10,
                    MinimalAnswer = 1,
                    Number = 3,
                    Value = "Question 3"
                },
                new NumericalIntegerQuestionDTO()
                {
                    CanBeSkipped = false,
                    MaximalAnswer = 10,
                    MinimalAnswer = 1,
                    Number = 4,
                    Value = "Question 4"
                },
                new TextualQuestionDTO()
                {
                    CanBeSkipped = false,
                    MaximalLength = 1000,
                    MinimalLength = 10,
                    Number = 5,
                    Value = "Question 5"
                }
            ],
            Id = "testSurvey2"
        };
        var result = await _surveyService.AddSurvey(survey, new User(){Id = 2});
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task ShouldReturnASurveyForAValidToken()
    {
        var survey = await _surveyService.GetSurvey("test");
        Assert.That(survey, Is.Not.Null);
    }
}