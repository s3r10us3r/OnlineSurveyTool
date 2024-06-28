using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class SurveyValidatorSurveyValidationTest
{
    private ISurveyValidator _surveyValidator;
    private SurveyDTO _sampleSurvey;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _surveyValidator = new SurveyValidator();
    }

    [SetUp]
    public void SetUpSampleSurvey()
    {
        _sampleSurvey = new SurveyDTO()
        {
            Name = "Test Survey",
            IsOpen = false,
            Id = "t",
            Questions = [
                new SingleChoiceQuestionDTO()
                {
                    Number = 1,
                    CanBeSkipped = false,
                    ChoiceOptions = [new() {Number = 1, Value = "Choice option one"}],
                    Value = "Question 1"
                },
                new MultipleChoiceQuestionDTO()
                {
                    Number = 2,
                    CanBeSkipped = false,
                    MaximalChoices = 1,
                    MinimalChoices = 1,
                    ChoiceOptions = [new() {Number = 1, Value = "Choice option one"}, new() {Number = 2, Value = "Choice option two"}],
                    Value = "Question 2"
                },
                new NumericalIntegerQuestionDTO()
                {
                    Number = 3,
                    CanBeSkipped = false,
                    MaximalAnswer = 10,
                    MinimalAnswer = 1,
                    Value = "Question 3"
                },
                new NumericalDoubleQuestionDTO()
                {
                    Number = 4,
                    CanBeSkipped = true,
                    MaximalAnswer = 10,
                    MinimalAnswer = 1,
                    Value = "Question 4"
                },
                new TextualQuestionDTO()
                {
                    Number = 5,
                    CanBeSkipped = true,
                    MinimalLength = 1,
                    MaximalLength = 1000,
                    Value = "Question 5"
                }
            ]
        };
    }

    [Test]
    public void ShouldValidateAValidSurveyCorrectly()
    {
        var result = _surveyValidator.ValidateSurvey(_sampleSurvey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(message, Is.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateASurveyWithNumberViolation()
    {
        _sampleSurvey.Questions[2].Number = 1;
        ShouldInvalidateAnInvalidSurvey();
    }
    
    [Test]
    public void ShouldInvalidateASurveyWithHighestQuestionNumberHigherThanQuestionCount()
    {
        _sampleSurvey.Questions[4].Number = 10000;
        ShouldInvalidateAnInvalidSurvey();
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidOpeningAndClosingDateTimes()
    {
        _sampleSurvey.OpeningDate = DateTime.Now;
        _sampleSurvey.ClosingDate = DateTime.Now.AddMinutes(-10);
        ShouldInvalidateAnInvalidSurvey();
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidSingleChoice()
    {
        var question1 = _sampleSurvey.Questions[0] as SingleChoiceQuestionDTO;
        question1!.ChoiceOptions = [];
        ShouldInvalidateAnInvalidSurvey();
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidMultipleChoice()
    {
        var question2 = _sampleSurvey.Questions[1] as MultipleChoiceQuestionDTO;
        question2!.MaximalChoices = 0;
        ShouldInvalidateAnInvalidSurvey();
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidNumericalInteger()
    {
        var question3 = _sampleSurvey.Questions[2] as NumericalIntegerQuestionDTO;
        question3!.MaximalAnswer = 0;
        ShouldInvalidateAnInvalidSurvey();
    }
    
    [Test]
    public void ShouldInvalidateASurveyWithInvalidNumericalDouble()
    {
        var question4 = _sampleSurvey.Questions[3] as NumericalDoubleQuestionDTO;
        question4!.MaximalAnswer = 0;
        ShouldInvalidateAnInvalidSurvey();
    }

    [Test]
    public void ShouldInvalidateASurveyWithInvalidTextual()
    {
        var question5 = _sampleSurvey.Questions[4] as TextualQuestionDTO;
        question5!.MaximalLength = 0;
        ShouldInvalidateAnInvalidSurvey();
    }
    
    private void ShouldInvalidateAnInvalidSurvey()
    {
        var result = _surveyValidator.ValidateSurvey(_sampleSurvey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
}