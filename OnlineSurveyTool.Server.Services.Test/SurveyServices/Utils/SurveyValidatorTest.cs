using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Utils;

[TestFixture]
public class SurveyValidatorTest
{
    private ISurveyValidator _surveyValidator;
    private SurveyDTO _survey;
    
    [SetUp]
    public void SetUp()
    {
        var config = ConfigurationCreator.MockConfig();
        var questionValidator = new QuestionValidator(config);
        _surveyValidator = new SurveyValidator(questionValidator);
        _survey = new SurveyDTO()
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

    [Test]
    public void ShouldValidateCorrectlyForAValidSurvey()
    {
        var result = _surveyValidator.ValidateSurvey(_survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(message, Is.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidQuestions()
    {
        _survey.Questions[0].Minimum = 1000;
        var result = _surveyValidator.ValidateSurvey(_survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
}