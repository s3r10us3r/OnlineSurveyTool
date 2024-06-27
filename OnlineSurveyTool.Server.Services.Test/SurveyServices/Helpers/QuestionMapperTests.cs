using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class QuestionMapperTests
{
    private QuestionMapper _questionMapper;
    [SetUp]
    public void SetUp()
    {
        _questionMapper = new QuestionMapper();
    }

    [Test]
    public void ShouldCorrectlyMapSingleChoice()
    {
        var question = new SingleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 2, Value = "Choice 2"}],
            Value = "Choose"
        };
        
        ShouldCorrectlyMapQuestion(question, QuestionType.SingleChoice);
    }

    [Test]
    public void ShouldCorrectlyMapMultipleChoice()
    {
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 2, Value = "Choice 2"}],
            MaximalChoices = 2,
            MinimalChoices = 1,
            Value = "Choose"
        };
        
        ShouldCorrectlyMapQuestion(question, QuestionType.MultipleChoice);
    }

    [Test]
    public void ShouldCorrectlyMapNumericalDouble()
    {
        var question = new NumericalDoubleQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            Value = "Type number",
            MinimalAnswer = 1,
            MaximalAnswer = 10
        };
        
        ShouldCorrectlyMapQuestion(question, QuestionType.NumericalDouble);
    }

    [Test]
    public void ShouldCorrectlyMapNumericalInteger()
    {
        var question = new NumericalIntegerQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            Value = "Type number",
            MinimalAnswer = 1,
            MaximalAnswer = 10
        };
        ShouldCorrectlyMapQuestion(question, QuestionType.NumericalInteger);
    }

    [Test]
    public void ShouldCorrectlyMapTextual()
    {
        var question = new TextualQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            Value = "Text",
            MaximalLength = 1000,
            MinimalLength = 10
        };
        
        ShouldCorrectlyMapQuestion(question, QuestionType.Textual);
    }
    
    private void ShouldCorrectlyMapQuestion(QuestionBase question, QuestionType type) 
    {
        var result = _questionMapper.MapDto(question);
        Assert.That(result.Type, Is.EqualTo(type));
    }
}