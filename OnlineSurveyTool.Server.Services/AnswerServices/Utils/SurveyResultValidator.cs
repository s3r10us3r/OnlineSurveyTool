using Microsoft.JSInterop.Infrastructure;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Utils;

public class SurveyResultValidator : ISurveyResultValidator
{
    private IUnitOfWork _uow;
    private ISurveyRepo _surveyRepo;
    
    public SurveyResultValidator(IUnitOfWork uow)
    {
        _uow = uow;
        _surveyRepo = uow.SurveyRepo;
    }
    
    public async Task<bool> ValidateSurveyResultDto(SurveyResultDTO dto)
    {
        var survey = await _surveyRepo.GetOne(dto.Id);
        if (survey is null)
            return false;
        HashSet<int> appeared = [];
        foreach (var answerDto in dto.Answers)
        {
            if (!appeared.Add(answerDto.Number))
                return false;
            var question = survey.Questions.FirstOrDefault(q => q.Number == answerDto.Number);
            if (question is null || !IsAnswerDtoValid(answerDto, question))
                return false;
        }

        return true;
    }

    public bool IsAnswerDtoValid(AnswerDTO dto, Question question)
    {
        return question.Type switch
        {
            QuestionType.SingleChoice => ValidateSingleChoice(dto, question),
            QuestionType.MultipleChoice => ValidateMultipleChoice(dto, question),
            QuestionType.NumericalInteger => ValidateNumericalInteger(dto, question),
            QuestionType.NumericalDouble => ValidateNumericalDouble(dto, question),
            QuestionType.Textual => ValidateTextual(dto, question),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private bool ValidateSingleChoice(AnswerDTO dto, Question question)
    {
        if (dto.ChosenOptions is null || dto.ChosenOptions.Count != 1)
            return false;

        var choice = dto.ChosenOptions[0];
        return IsChoiceOptionValid(choice, question);
    }

    private bool ValidateMultipleChoice(AnswerDTO dto, Question question)
    {
        if (dto.ChosenOptions is null || dto.ChosenOptions.Count != 1)
            return false;
        if (dto.ChosenOptions.ToHashSet().Count < dto.ChosenOptions.Count)
            return false;
        if (dto.ChosenOptions.Count < question.Minimum || dto.ChosenOptions.Count > question.Maximum)
            return false;
        
        return dto.ChosenOptions.All(co => IsChoiceOptionValid(co, question));
    }
    
    private bool IsChoiceOptionValid(int coNum, Question question)
    {
        return question.ChoiceOptions!.FirstOrDefault(c => c.Number == coNum) is not null;
    }

    private bool ValidateNumericalInteger(AnswerDTO dto, Question question)
    {
        return dto.Answer is not null &&
               IsInteger((double)(dto.Answer)) &&
               dto.Answer <= question.Maximum &&
               dto.Answer >= question.Minimum;
    }

    private bool IsInteger(double num)
    {
        return num % 1 == 0 && num >= 0;
    }

    private bool ValidateNumericalDouble(AnswerDTO dto, Question question)
    {
        return dto.Answer is not null &&
               dto.Answer <= question.Maximum &&
               dto.Answer >= question.Minimum;
    }

    private bool ValidateTextual(AnswerDTO dto, Question question)
    {
        return dto.TextAnswer is not null &&
               dto.TextAnswer.Length >= question.Minimum &&
               dto.TextAnswer.Length <= question.Maximum;
    }
}