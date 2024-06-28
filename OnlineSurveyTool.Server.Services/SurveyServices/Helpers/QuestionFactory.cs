using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

public class QuestionFactory : IQuestionFactory
{
    public QuestionBase MakeQuestionBase(Question question)
    {
        return question.Type switch
        {
            QuestionType.SingleChoice => MakeSingleChoice(question),
            QuestionType.MultipleChoice => MakeMultipleChoiceQuestionDto(question),
            QuestionType.NumericalInteger => MakeNumericalIntegerQuestionDto(question),
            QuestionType.NumericalDouble => MakeNumericalDoubleQuestionDto(question),
            QuestionType.Textual => MakeTextualQuestionDto(question),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private SingleChoiceQuestionDTO MakeSingleChoice(Question question)
    {
        return new SingleChoiceQuestionDTO()
        {
            Value = question.Value,
            Number = question.Number,
            CanBeSkipped = question.CanBeSkipped,
            ChoiceOptions = question.ChoiceOptions!.Select(ChoiceOptionToDto).ToList(),
            Id = question.ExternalId
        };
    }

    private MultipleChoiceQuestionDTO MakeMultipleChoiceQuestionDto(Question question)
    {
        return new MultipleChoiceQuestionDTO()
        {
            Value = question.Value,
            Number = question.Number,
            CanBeSkipped = question.CanBeSkipped,
            ChoiceOptions = question.ChoiceOptions!.Select(ChoiceOptionToDto).ToList(),
            MinimalChoices = (int)question.Minimum!,
            MaximalChoices = (int)question.Maximum!,
            Id = question.ExternalId
        };
    }

    private NumericalDoubleQuestionDTO MakeNumericalDoubleQuestionDto(Question question)
    {
        return new NumericalDoubleQuestionDTO()
        {
            Value = question.Value,
            Number = question.Number,
            CanBeSkipped = question.CanBeSkipped,
            MaximalAnswer = (double)question.Maximum!,
            MinimalAnswer = (double)question.Minimum!,
            Id = question.ExternalId
        };
    }

    private NumericalIntegerQuestionDTO MakeNumericalIntegerQuestionDto(Question question)
    {
        return new NumericalIntegerQuestionDTO()
        {
            Value = question.Value,
            Number = question.Number,
            CanBeSkipped = question.CanBeSkipped,
            MaximalAnswer = (int)question.Maximum!,
            MinimalAnswer = (int)question.Minimum!,
            Id = question.ExternalId
        };
    }

    private TextualQuestionDTO MakeTextualQuestionDto(Question question)
    {
        return new TextualQuestionDTO()
        {
            Value = question.Value,
            Number = question.Number,
            CanBeSkipped = question.CanBeSkipped,
            MaximalLength = (int)question.Maximum!,
            MinimalLength = (int)question.Minimum!,
            Id = question.ExternalId
        };
    }
    
    private ChoiceOptionDTO ChoiceOptionToDto(ChoiceOption choiceOption)
    {
        return new()
        {
            Number = choiceOption.Number,
            Value = choiceOption.Value,
            Id = choiceOption.ExternalId
        };
    }
}