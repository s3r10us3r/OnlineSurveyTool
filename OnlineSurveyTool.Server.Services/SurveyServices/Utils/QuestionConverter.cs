using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Extensions;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class QuestionConverter : IQuestionConverter
{
    private IGuidGenerator _guidGenerator;
    private IChoiceOptionConverter _choiceOptionConverter;
    
    public QuestionConverter(IGuidGenerator guidGenerator, IChoiceOptionConverter choiceOptionConverter)
    {
        _guidGenerator = guidGenerator;
        _choiceOptionConverter = choiceOptionConverter;
    }
    
    public Question DtoToQuestion(QuestionDTO dto)
    {
        return new Question()
        {
            Number = dto.Number,
            Id = dto.Id ?? _guidGenerator.GenerateGuid(),
            Value = dto.Value,
            Type = QuestionTypeHelper.FromString(dto.Type) ?? throw new ArgumentException("Nonexistent type"),
            Minimum = dto.Minimum,
            Maximum = dto.Maximum,
            CanBeSkipped = dto.CanBeSkipped,
            ChoiceOptions = dto.ChoiceOptions?.Select(_choiceOptionConverter.DtoToChoiceOption),
        };
    }

    public QuestionDTO QuestionToDto(Question question)
    {
        return new QuestionDTO()
        {
            Number = question.Number,
            Id = question.Id,
            Value = question.Value,
            Type = question.Type.ToStringFriendly(),
            Minimum = question.Minimum,
            Maximum = question.Maximum,
            CanBeSkipped = question.CanBeSkipped,
            ChoiceOptions = question.ChoiceOptions?.Select(_choiceOptionConverter.ChoiceOptionToDto).ToList()
        };
    }
    
}