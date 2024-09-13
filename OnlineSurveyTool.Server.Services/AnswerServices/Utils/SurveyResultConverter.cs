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
            Answers = ToAnswerList(dto.Answers, survey.Questions),
            TimeStamp = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString()
        };
    }

    private ICollection<Answer> ToAnswerList(ICollection<AnswerDTO> dtos, ICollection<Question> questions)
    {
         return dtos.Join(
            questions,
            a => a.Number,
            q => q.Number,
            DtoToAnswer).ToList();
    }

    private Answer DtoToAnswer(AnswerDTO dto, Question question)
    {
        var answer = new Answer()
        {
            QuestionNumber = question.Number,
        };
        
        return question.Type switch
        {
            QuestionType.SingleChoice => ToSingleChoice(answer, dto, question),
            QuestionType.MultipleChoice => ToMultipleChoice(answer, dto, question),
            QuestionType.NumericalInteger => ToNumerical(answer, dto, question),
            QuestionType.NumericalDouble => ToNumerical(answer, dto, question),
            QuestionType.Textual => ToTextual(answer, dto, question),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Answer ToSingleChoice(Answer answer, AnswerDTO dto, Question question)
    {
        answer.Discriminator = AnswerDiscriminator.SingleChoice;
        answer.ChoiceOption = question.ChoiceOptions.First(co => co.Number == dto.ChosenOptions!.Single());
        return answer;
    }

    private Answer ToMultipleChoice(Answer answer, AnswerDTO dto, Question question)
    {
        answer.Discriminator = AnswerDiscriminator.MultipleChoice;
        answer.AnswerOptions = dto.ChosenOptions.Select(num => new AnswerOption()
        {
            Answer = answer,
            ChoiceOption = question.ChoiceOptions.First(co => co.Number == num)
        })
        .ToList();
        return answer;
    }

    private Answer ToNumerical(Answer answer, AnswerDTO dto, Question question)
    {
        answer.Discriminator = AnswerDiscriminator.Numerical;
        answer.NumericalAnswer = dto.Answer;
        return answer;
    }

    private Answer ToTextual(Answer answer, AnswerDTO dto, Question question)
    {
        answer.Discriminator = AnswerDiscriminator.Textual;
        answer.TextAnswer = dto.TextAnswer;
        return answer;
    }
}
