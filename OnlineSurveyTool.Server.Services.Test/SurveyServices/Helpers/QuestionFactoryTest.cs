using NuGet.Frameworks;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class QuestionFactoryTest
{
    private IQuestionFactory _questionFactory;
    private ISurveyValidator _surveyValidator;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _questionFactory = new QuestionFactory();
        _surveyValidator = new SurveyValidator();
    }

    [Test]
    public void ShouldCorrectlyConvertToSingleChoice()
    {
        var question = new Question()
        {
            CanBeSkipped = true,
            ChoiceOptions =
                [new() { Id = 1, Number = 1, Value = "Option 1" }, new() { Id = 2, Number = 2, Value = "Option 2" }],
            Id = 1,
            Number = 1,
            Type = QuestionType.SingleChoice
        };
        ShouldCorrectlyConvert<SingleChoiceQuestionDTO>(question);
    }

    [Test]
    public void ShouldCorrectlyConvertToMultipleChoice()
    {
        var question = new Question()
        {
            CanBeSkipped = true,
            Minimum = 1,
            Maximum = 2,
            ChoiceOptions =
                [new() { Id = 1, Number = 1, Value = "Option 1" }, new() { Id = 2, Number = 2, Value = "Option 2" }],
            Id = 1,
            Number = 1,
            Type = QuestionType.MultipleChoice
        };
        ShouldCorrectlyConvert<MultipleChoiceQuestionDTO>(question);
    }

    [Test]
    public void ShouldCorrectlyConvertToNumericalInteger()
    {
        var question = new Question()
        {
            CanBeSkipped = true,
            Minimum = 1,
            Maximum = 10,
            Id = 1,
            Number = 1,
            Type = QuestionType.NumericalInteger
        };
        ShouldCorrectlyConvert<NumericalIntegerQuestionDTO>(question);
    }
    
    [Test]
    public void ShouldCorrectlyConvertToNumericalDouble()
    {
        var question = new Question()
        {
            CanBeSkipped = true,
            Minimum = 1,
            Maximum = 10,
            Id = 1,
            Number = 1,
            Type = QuestionType.NumericalDouble
        };
        ShouldCorrectlyConvert<NumericalDoubleQuestionDTO>(question);
    }

    [Test]
    public void ShouldCorrectlyConvertToTextual()
    {
        var question = new Question()
        {
            CanBeSkipped = true,
            Minimum = 1,
            Maximum = 1000,
            Id = 1,
            Number = 1,
            Type = QuestionType.Textual
        };
        ShouldCorrectlyConvert<TextualQuestionDTO>(question);
    }
    
    private void ShouldCorrectlyConvert<T>(Question question) where T : QuestionBase
    {
        var result = _questionFactory.MakeQuestionBase(question);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<T>());
            Assert.That(_surveyValidator.ValidateQuestion(result, out var message), Is.True, message);
        });
    }
}