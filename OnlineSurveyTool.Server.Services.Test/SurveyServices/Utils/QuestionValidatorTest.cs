using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Utils;

[TestFixture]
public class QuestionValidatorTest
{
    private IQuestionValidator _questionValidator;
    private List<QuestionDTO> _questionDtos;
    
    [SetUp]
    public void SetUp()
    {
        _questionValidator = new QuestionValidator(ConfigurationCreator.MockConfig());
        _questionDtos =
        [
            new()
            {
                Number = 0,
                Value = "Question 1",
                Type = "Single Choice",
                CanBeSkipped = true,
                ChoiceOptions = [
                    new() {Number = 0, Value = "Option 1"}, new() {Number = 1, Value = "Option 2"}, 
                    new() {Number = 2, Value = "Option 3"}
                ],
            },
            new()
            {
                Number = 1,
                Value = "Question 2",
                Type = "Multiple Choice",
                CanBeSkipped = true,
                ChoiceOptions = [
                    new() {Number = 0, Value = "Option 1"}, new() {Number = 1, Value = "Option 2"}, 
                    new() {Number = 2, Value = "Option 3"}
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
        ];
    }

    [Test]
    public void ShouldValidateAValidSingleChoice()
    {
        ShouldValidateAValidQuestion(_questionDtos[0]);
    }
    
    [Test]
    public void ShouldValidateAValidMultipleChoice()
    {
        ShouldValidateAValidQuestion(_questionDtos[1]);
    }
    
    
    [Test]
    public void ShouldValidateAValidNumericalDouble()
    {
        ShouldValidateAValidQuestion(_questionDtos[2]);
    }
    
    [Test]
    public void ShouldValidateAValidNumericalInteger()
    {
        ShouldValidateAValidQuestion(_questionDtos[3]);
    }

    [Test]
    public void ShouldValidateAValidTextual()
    {
        ShouldValidateAValidQuestion(_questionDtos[4]);
    }

    [Test]
    public void ShouldInvalidateAnInvalidSingleChoice()
    {
        var question = _questionDtos[0];
        question.ChoiceOptions![0].Number = 10;
        ShouldInvalidateAnInvalidQuestion(question);
    }
    
    [Test]
    public void ShouldInvalidateAnInvalidMultipleChoice()
    {
        var question = _questionDtos[1];
        question.Maximum = 1000;
        ShouldInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldInvalidateAnInvalidNumericalDouble()
    {
        var question = _questionDtos[2];
        question.Minimum = 10000;
        ShouldInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldInvalidateAnInvalidNumericalInteger()
    {
        var question = _questionDtos[3];
        question.Minimum = -1.5;
        ShouldInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldInvalidateAnInvalidTextual()
    {
        var question = _questionDtos[4];
        question.Maximum = 10000000;
        ShouldInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldValidateAValidQuestionList()
    {
        var result = _questionValidator.ValidateQuestions(_questionDtos, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, message);
            Assert.That(message, Is.Empty, message);
        });
    }

    [Test]
    public void ShouldInvalidateAnInvalidQuestionList()
    {
        _questionDtos[0].Number = 1000;
        var result = _questionValidator.ValidateQuestions(_questionDtos, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
        
    private void ShouldValidateAValidQuestion(QuestionDTO questionDto)
    {
        var result = _questionValidator.ValidateQuestion(questionDto, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, message);
            Assert.That(message, Is.Empty, message);
        });
    }

    private void ShouldInvalidateAnInvalidQuestion(QuestionDTO questionDto)
    {
        var result = _questionValidator.ValidateQuestion(questionDto, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
}