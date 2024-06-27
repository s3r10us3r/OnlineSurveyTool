using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;
using OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers.Internals;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class SurveyConverterTests
{
    private ISurveyConverter _surveyConverter;
    private SurveyDTO _surveyDto;
    private Survey _survey;
    private SurveyComparer _surveyComparer;
    
    [SetUp]
    public void SetUp()
    {
        var questionFactory = new QuestionFactory();
        var questionMapper = new QuestionMapper();
        _surveyConverter = new SurveyConverter(questionFactory, questionMapper);
        _surveyComparer = new SurveyComparer();
        MakeTestSurveyDto();
        MakeTestSurvey();
    }

    [Test]
    public void ShouldCorrectlyConvertSurveyToDto()
    {
        var dto = _surveyConverter.SurveyToDto(_survey);
        _surveyComparer.CompareSurveys(dto, _survey);
    }

    [Test]
    public void ShouldCorrectlyConvertDtoToSurvey()
    {
        var survey = _surveyConverter.DtoToSurvey(_surveyDto, new User() { Id = 1, Login = "login" });
        _surveyComparer.CompareSurveys(_surveyDto, survey);
    }
    
    private void MakeTestSurveyDto()
    {
        _surveyDto = new SurveyDTO()
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
                    ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 1, Value = "Choice 2"}],
                    Number = 1,
                    Value = "question 1"
                },
                new MultipleChoiceQuestionDTO()
                {
                    CanBeSkipped = false,
                    ChoiceOptions = [new ChoiceOptionDTO(){Number = 1, Value = "Choice 1"}, new ChoiceOptionDTO(){Number = 1, Value = "Choice 2"}],
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
            Token = "testSurvey"
        };
    }

    private void MakeTestSurvey()
    {
        _survey = new Survey()
        {
            Id = 1,
            OpeningDate = DateTime.Today,
            ClosingDate = DateTime.Today.AddDays(1),
            IsArchived = false,
            IsOpen = true,
            Name = "Survey",
            Questions = [
                new Question()
                {
                    Type = QuestionType.SingleChoice,
                    CanBeSkipped = false,
                    ChoiceOptions = [new ChoiceOption {Number = 1, Value = "Choice 1"}, new ChoiceOption {Number = 2, Value = "Choice 2"}],
                    Number = 1,
                    Value = "Question 1"
                },
                new Question()
                {
                    Type = QuestionType.MultipleChoice,
                    CanBeSkipped = false,
                    ChoiceOptions = [new ChoiceOption {Number = 1, Value = "Choice 1"}, new ChoiceOption {Number = 2, Value = "Choice 2"}],
                    Number = 2,
                    Value = "Question 2",
                    Minimum = 1,
                    Maximum = 2
                },
                new Question()
                {
                    Type = QuestionType.NumericalDouble,
                    CanBeSkipped = false,
                    Number = 3,
                    Minimum = 1,
                    Maximum = 10,
                    Value = "Question 3"
                },
                new Question()
                {
                    Type = QuestionType.NumericalInteger,
                    CanBeSkipped = false,
                    Number = 4,
                    Minimum = 1,
                    Maximum = 10,
                    Value = "Question 4"
                },
                new Question()
                {
                    Type = QuestionType.Textual,
                    CanBeSkipped = false,
                    Number = 5,
                    Minimum = 10,
                    Maximum = 1000,
                    Value = "Question 5"
                }
            ]
        };
    }
}