using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices;

[TestFixture]
public class SurveyServiceTest
{
    private ISurveyService _surveyService;
    private SurveyDTO _sampleSurvey;
    private SurveyEditDto _sampleEdit;
    
    [SetUp]
    public void SetUp()
    {
        var uow = new UnitOfWorkMock();
        var questionValidator = new QuestionValidator(ConfigurationCreator.MockConfig());
        var surveyValidator = new SurveyValidator(questionValidator);
        var editSurveyValidator = new EditSurveyValidator(questionValidator, surveyValidator);
        var logger = new LoggerMock<Services.SurveyServices.SurveyService>();
        _surveyService = new Services.SurveyServices.SurveyService(uow, surveyValidator, editSurveyValidator, logger);
        _sampleSurvey = ProvideSampleSurvey();
        _sampleEdit = ProvideSampleEdit();
    }

    [Test]
    public async Task AddShouldSucceedForAValidSurveyAndUser()
    {
        var result = await _surveyService.AddSurvey("TestUser1", _sampleSurvey);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task AddShouldFailForANonexistentUser()
    {
        var result = await _surveyService.AddSurvey("NoUser2137", _sampleSurvey);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Message, Is.Not.Empty);
        });
    }

    [Test]
    public async Task AddShouldFailForAnInvalidSurvey()
    {
        _sampleSurvey.Questions[0].Number = 1000;
        var result = await _surveyService.AddSurvey("TestUser1", _sampleSurvey);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Message, Is.Not.Empty);
        });
    }

    [Test]
    public async Task EditShouldSucceedForAValidSurvey()
    {
        var result = await _surveyService.EditSurvey("TestUser1", _sampleEdit);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
    }
    
    private SurveyDTO ProvideSampleSurvey()
    {
        return new()
        {
            Name = "Survey",
            Questions =
            [
                new()
                {
                    Number = 0,
                    Value = "Question 1",
                    Type = "Single Choice",
                    CanBeSkipped = true,
                    ChoiceOptions =
                    [
                        new() { Number = 0, Value = "Option 1" }, new() { Number = 1, Value = "Option 2" },
                        new() { Number = 2, Value = "Option 3" }
                    ],
                },
                new()
                {
                    Number = 1,
                    Value = "Question 2",
                    Type = "Multiple Choice",
                    CanBeSkipped = true,
                    ChoiceOptions =
                    [
                        new() { Number = 0, Value = "Option 1" }, new() { Number = 1, Value = "Option 2" },
                        new() { Number = 2, Value = "Option 3" }
                    ],
                    Minimum = 1,
                    Maximum = 3
                },
                new()
                {
                    Number = 2,
                    Value = "Question 3",
                    Type = "Numerical Double",
                    CanBeSkipped = false,
                    Minimum = 0.1234,
                    Maximum = 10.123,
                },
                new()
                {
                    Number = 3,
                    Value = "Question 4",
                    Type = "Numerical Integer",
                    CanBeSkipped = false,
                    Minimum = -10,
                    Maximum = 10
                },
                new()
                {
                    Number = 4,
                    Value = "Question 5",
                    Type = "Textual",
                    CanBeSkipped = false,
                    Minimum = 10,
                    Maximum = 500
                }
            ],
        };
    }

    private SurveyEditDto ProvideSampleEdit()
    {
        return new()
        {
            Id = "survey1",
            Name = "New survey name",
            DeletedQuestions = ["question5"],
            NewQuestions =
            [
                new()
                {
                    Value = "New question",
                    Number = 3,
                    Maximum = 10,
                    Minimum = 1,
                    CanBeSkipped = true,
                    Type = "Numerical Integer"
                }
            ],
            EditedQuestions =
            [
                new()
                {
                    Id = "question4",
                    Number = 4,
                }
            ]
        };
    }
}