using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Utils;

public class SurveyResultConverter : ISurveyResultConverter
{
    private readonly IUnitOfWork _uow;
    private readonly ISurveyRepo _surveyRepo;
    
    public SurveyResultConverter(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
        _surveyRepo = _uow.SurveyRepo;
    }
    
    public async Task<SurveyResult> SurveyResultToModel(SurveyResultDTO dto)
    {
        var survey = await _surveyRepo.GetOne(dto.Id);
        if (survey is null)
            throw new InvalidDataException($"Survey with id {dto.Id} does not exist!");
        
        return new SurveyResult
        {
            SurveyId = dto.Id,
            Answers = await ToAnswerList(dto.Answers, survey.Questions),
            TimeStamp = DateTime.UtcNow
        };
    }

    private async Task<ICollection<Answer>> ToAnswerList(ICollection<AnswerDTO> dtos, ICollection<Question> questions)
    {
        var answerTasks = dtos.Join(
            questions,
            a => a.Number,
            q => q.Number,
            async (a, q) => await DtoToAnswer(a, q));

        var answers = await Task.WhenAll(answerTasks);
        return answers.ToList();
    }

    private async Task<Answer> DtoToAnswer(AnswerDTO dto, Question question)
    {
        return question.Type switch
        {
            QuestionType.SingleChoice => await ToSingleChoice(dto, question),
            QuestionType.MultipleChoice => await ToMultipleChoice(dto, question),
            QuestionType.NumericalInteger => await ToNumerical(dto, question),
            QuestionType.NumericalDouble => await ToNumerical(dto, question),
            QuestionType.Textual => await ToTextual(dto, question),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private async Task<Answer> ToSingleChoice(AnswerDTO dto, Question question)
    {
        return new AnswerSingleChoice()
        {
            QuestionNumber = question.Number,
            ChoiceOptionId = question.ChoiceOptions!.First(c => c.Number == dto.ChosenOptions!.Single()).Id
        };
    }

    private async Task<Answer> ToMultipleChoice(AnswerDTO dto, Question question)
    {
        return new AnswerMultipleChoice()
        {
            QuestionNumber = question.Number,
            ChoiceOptions = dto.ChosenOptions!
                .Select(num => question.ChoiceOptions!.First(co => co.Number == num)).ToList()
        };
    }
    
    private async Task<Answer> ToNumerical(AnswerDTO dto, Question question)
    {
        return new AnswerNumerical
        {
            QuestionNumber = question.Number,
            Answer = (double)dto.Answer!
        };
    }
    
    private async Task<Answer> ToTextual(AnswerDTO dto, Question question)
    {
        return new AnswerTextual
        {
            QuestionNumber = question.Number,
            Text = dto.TextAnswer!
        };
    }
}
