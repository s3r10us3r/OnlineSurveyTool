using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Extensions;

public static class Conversions
{
    public static ChoiceOption DtoToChoiceOption(this ChoiceOptionDTO dto)
    {
        return new ChoiceOption()
        {
            Number = dto.Number,
            Id = dto.Id ?? GuidGenerator.GenerateGuid(),
            Value = dto.Value
        };
    }

    public static ChoiceOptionDTO ChoiceOptionToDto(this ChoiceOption choiceOption)
    {
        return new ChoiceOptionDTO()
        {
            Number = choiceOption.Number,
            Id = choiceOption.Id,
            Value = choiceOption.Value
        };
    }

    public static Question DtoToQuestion(this QuestionDTO dto)
    {
        return new Question()
        {
            Number = dto.Number,
            Id = dto.Id ?? GuidGenerator.GenerateGuid(),
            Value = dto.Value,
            Type = QuestionTypeHelper.FromString(dto.Type) ?? throw new ArgumentException("Nonexistent type"),
            Minimum = dto.Minimum,
            Maximum = dto.Maximum,
            CanBeSkipped = dto.CanBeSkipped,
            ChoiceOptions = dto.ChoiceOptions?.Select(c => c.DtoToChoiceOption())
        };
    }

    public static QuestionDTO QuestionToDto(this Question question)
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
            ChoiceOptions = question.ChoiceOptions?.Select(c => c.ChoiceOptionToDto()).ToList()
        };
    }

    public static Survey DtoToSurvey(this SurveyDTO surveyDto)
    {
        return new Survey()
        {
            Id = surveyDto.Id ?? GuidGenerator.GenerateGuid(),
            Name = surveyDto.Name,
            Questions = surveyDto.Questions.Select(q => q.DtoToQuestion())
        };
    }

    public static SurveyDTO SurveyToDto(this Survey survey)
    {
        return new SurveyDTO()
        {
            Id = survey.Id,
            Name = survey.Name,
            Questions = survey.Questions.Select(q => q.QuestionToDto()).ToList()
        };
    }
}