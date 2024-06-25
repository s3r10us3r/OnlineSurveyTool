using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class SurveyValidatorQuestionValidationTest
{
    private ISurveyValidator _surveyValidator;

    [OneTimeSetUp]
    public void SetUp()
    {
        _surveyValidator = new SurveyValidator();
    }

    [Test]
    public void ShouldCorrectlyValidateAValidSingleChoice()
    {
        var question = new SingleChoiceQuestionDTO()
        {
            Number = 1,
            Value = "Choose an option",
            CanBeSkipped = false,
            ChoiceOptions = [new ChoiceOptionDTO {Number = 1, Value = "A choice option"}]
        };
        ShouldCorrectlyValidateAValidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyValidateAValidMultipleChoice()
    { 
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            MaximalChoices = 2,
            MinimalChoices = 1,
            CanBeSkipped = false,
            Value = "Choose many options.",
            ChoiceOptions =
            [
                new() { Number = 1, Value = "Choice option 1" },
                new() { Number = 2, Value = "Choice option 2" }
            ]
        };

        ShouldCorrectlyValidateAValidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyValidateAValidNumericalDouble()
    {
        var question = new NumericalDoubleQuestionDTO()
        {
            Number = 1,
            MaximalAnswer = 10.5,
            MinimalAnswer = 0.8,
            CanBeSkipped = false,
            Value = "Choose a number"
        };
        
        ShouldCorrectlyValidateAValidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyValidateAValidNumericalInteger()
    {
        var question = new NumericalIntegerQuestionDTO()
        {
            Number = 1,
            MaximalAnswer = 10,
            MinimalAnswer = 2,
            CanBeSkipped = false,
            Value = "Choose a number"
        };
        
        ShouldCorrectlyValidateAValidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyValidateAValidTextual()
    {
        var question = new TextualQuestionDTO()
        {
            Number = 1,
            MaximalLength = 200,
            MinimalLength = 100,
            CanBeSkipped = false,
            Value = "Write text"
        };
        
        ShouldCorrectlyValidateAValidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateSingleChoiceWithNoChoices()
    {
        var question = new SingleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            ChoiceOptions = [],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateMultipleChoiceWithNoChoices()
    {
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            MinimalChoices = 1,
            MaximalChoices = 10,
            CanBeSkipped = false,
            ChoiceOptions = [],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateSingleChoiceWithNumberViolation()
    {
        var question = new SingleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            ChoiceOptions = [new() {Number = 1, Value = "A choice option"}, new() {Number = 1, Value = "A different choice option"}],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateMultipleChoiceWithNumberViolation()
    {
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalChoices = 1,
            MaximalChoices = 2,
            ChoiceOptions = [new() {Number = 1, Value = "A choice option"}, new() {Number = 1, Value = "A different choice option"}],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateMultipleChoiceWithMaximalChoicesHigherThenTheNumberOfPossibleChoices()
    {
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalChoices = 3,
            MaximalChoices = 1,
            ChoiceOptions = [new() {Number = 1, Value = "A choice option"}, new() {Number = 2, Value = "A different choice option"}],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateMultipleChoiceWithMaximalChoicesSmallerThanMinimalChoices()
    {
        var question = new MultipleChoiceQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalChoices = 2,
            MaximalChoices = 1,
            ChoiceOptions = [new() {Number = 1, Value = "A choice option"}, new() {Number = 2, Value = "A different choice option"}],
            Value = "Choose an option"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateNumericalDoubleWithMaximumSmallerThanMinimum()
    {
        var question = new NumericalDoubleQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalAnswer = 10,
            MaximalAnswer = 8,
            Value = "Invalid"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateNumericalIntegerWithMaximumSmallerThanMinimum()
    {
        var question = new NumericalIntegerQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalAnswer = 10,
            MaximalAnswer = 8,
            Value = "Invalid"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }

    [Test]
    public void ShouldCorrectlyInvalidateTextualWithMinimumSmallerThanMaximum()
    {
        var question = new TextualQuestionDTO()
        {
            Number = 1,
            CanBeSkipped = false,
            MinimalLength = 1000,
            MaximalLength = 500,
            Value = "Text"
        };
        
        ShouldCorrectlyInvalidateAnInvalidQuestion(question);
    }
    
    private void ShouldCorrectlyValidateAValidQuestion<T>(T question) where T : QuestionBase
    {
        var result = _surveyValidator.ValidateQuestion(question, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, message);
            Assert.That(message, Is.Empty, message);
        });
    }

    private void ShouldCorrectlyInvalidateAnInvalidQuestion<T>(T question) where T : QuestionBase
    {
        var result = _surveyValidator.ValidateQuestion(question, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
}