using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

public class QuestionMapper : IQuestionMapper
{
    public Question MapDto(QuestionBase question) => question switch
    {
        MultipleChoiceQuestionDTO multipleChoiceQuestionDto => MultipleChoiceToQuestion(multipleChoiceQuestionDto),
        NumericalDoubleQuestionDTO numericalDoubleQuestionDto => NumericalDoubleToQuestion(numericalDoubleQuestionDto),
        NumericalIntegerQuestionDTO numericalIntegerQuestionDto => NumericalIntegerToQuestion(numericalIntegerQuestionDto),
        SingleChoiceQuestionDTO singleChoiceQuestionDto => SingleChoiceToQuestion(singleChoiceQuestionDto),
        TextualQuestionDTO textualQuestionDto => TextualToQuestion(textualQuestionDto),
        _ => throw new ArgumentOutOfRangeException(nameof(question), question, null)
    };

    private Question MultipleChoiceToQuestion(MultipleChoiceQuestionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        Type = QuestionType.MultipleChoice,
        CanBeSkipped = dto.CanBeSkipped,
        Minimum = dto.MinimalChoices,
        Maximum = dto.MaximalChoices,
        ChoiceOptions = dto.ChoiceOptions.Select(DtoToChoiceOption),
        ExternalId = dto.Id,
    };

    private Question SingleChoiceToQuestion(SingleChoiceQuestionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        Type = QuestionType.SingleChoice,
        CanBeSkipped = dto.CanBeSkipped,
        ChoiceOptions = dto.ChoiceOptions.Select(DtoToChoiceOption),
        ExternalId = dto.Id,
    };
    
    private Question NumericalDoubleToQuestion(NumericalDoubleQuestionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        Type = QuestionType.NumericalDouble,
        CanBeSkipped = dto.CanBeSkipped,
        Minimum = dto.MinimalAnswer,
        Maximum = dto.MaximalAnswer,
        ExternalId = dto.Id,
    };
    
    private Question NumericalIntegerToQuestion(NumericalIntegerQuestionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        Type = QuestionType.NumericalInteger,
        CanBeSkipped = dto.CanBeSkipped,
        Minimum = dto.MinimalAnswer,
        Maximum = dto.MaximalAnswer,
        ExternalId = dto.Id,
    };

    private Question TextualToQuestion(TextualQuestionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        Type = QuestionType.Textual,
        CanBeSkipped = dto.CanBeSkipped,
        Minimum = dto.MinimalLength,
        Maximum = dto.MaximalLength,
        ExternalId = dto.Id,
    };
    
    private ChoiceOption DtoToChoiceOption(ChoiceOptionDTO dto) => new()
    {
        Number = dto.Number,
        Value = dto.Value,
        ExternalId = dto.Id
    };

}