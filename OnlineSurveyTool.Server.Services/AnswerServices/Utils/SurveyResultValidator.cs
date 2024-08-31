using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

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
    
    public async Task<IResult> ValidateSurveyResultDto(SurveyResultDTO dto)
    {
        var survey = await _surveyRepo.GetOne(dto.Id);
        if (survey is null)
            return Result.Failure("Survey with this id does not exist.");
        HashSet<int> appeared = [];
        foreach (var answerDto in dto.Answers)
        {
            if (!appeared.Add(answerDto.Number))
                return Result.Failure($"Multiple answers for question {answerDto.Number}");
            
            var question = survey.Questions.FirstOrDefault(q => q.Number == answerDto.Number);
            if (question is null)
                return Result.Failure($"Question with number {answerDto.Number} does not exist.");
            
            var dtoValidRes = IsAnswerDtoValid(answerDto, question);
            if (dtoValidRes.IsFailure)
                return dtoValidRes;
        }

        return Result.Success();
    }

    public IResult IsAnswerDtoValid(AnswerDTO dto, Question question)
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

    private IResult ValidateSingleChoice(AnswerDTO dto, Question question)
    {
        if (dto.ChosenOptions is null || dto.ChosenOptions.Count == 0)
            return Result.Failure($"There is no chosen option in single choice answer {dto.Number}");
        if (dto.ChosenOptions.Count != 1)
            return Result.Failure($"There has been chosen more than one option in single choice answer {dto.Number}");
        
        var choice = dto.ChosenOptions[0];
        if (question.ChoiceOptions is null)
            return Result.Failure($"Choice options in question number {question.Number} are null");

        if (question.ChoiceOptions.Count == 0)
            return Result.Failure($"Choice options in question number {question.Number} are empty question id {question.Id}");
        
        if (!IsChoiceOptionValid(choice, question))
            return Result.Failure($"Chosen option is not valid is single choice answer {dto.Number}");

        return Result.Success();
    }

    private IResult ValidateMultipleChoice(AnswerDTO dto, Question question)
    {
        dto.ChosenOptions ??= [];
        
        if (dto.ChosenOptions.ToHashSet().Count < dto.ChosenOptions.Count)
            return Result.Failure($"Same option has been chosen more than once in answer {dto.Number}");
        if (dto.ChosenOptions.Count < question.Minimum || dto.ChosenOptions.Count > question.Maximum)
            return Result.Failure($"Number of chosen options {question.ChoiceOptions!.Count} is out of range in answer {dto.Number}");

        if (!dto.ChosenOptions.All(co => IsChoiceOptionValid(co, question)))
            return Result.Failure($"Some chosen options are not valid in answer {dto.Number}");

        return Result.Success();
    }
    
    private bool IsChoiceOptionValid(int coNum, Question question)
    {
        return question.ChoiceOptions!.FirstOrDefault(c => c.Number == coNum) is not null;
    }

    private IResult ValidateNumericalInteger(AnswerDTO dto, Question question)
    {
        if (dto.Answer is null)
            return Result.Failure($"Answer {dto.Number} has no Answer field");

        if (!IsInteger((double)dto.Answer))
            return Result.Failure($"Answer {dto.Number} must be an integer.");

        if (dto.Answer > question.Maximum || dto.Answer < question.Minimum)
            return Result.Failure($"Answer {dto.Number} is out of range, number: {dto.Answer}, minimum: {question.Minimum}, maximum: {question.Maximum}");

        return Result.Success();
    }

    private bool IsInteger(double num)
    {
        return num % 1 == 0 && num >= 0;
    }

    private IResult ValidateNumericalDouble(AnswerDTO dto, Question question)
    {
        if (dto.Answer is null)
             return Result.Failure($"Answer {dto.Number} has no Answer field");
     
        if (dto.Answer > question.Maximum || dto.Answer < question.Minimum)
            return Result.Failure($"Answer {dto.Number} is out of range, number: {dto.Answer}, minimum: {question.Minimum}, maximum: {question.Maximum}");
     
        return Result.Success();
    }

    private IResult ValidateTextual(AnswerDTO dto, Question question)
    {
        var x = dto.TextAnswer is not null &&
               dto.TextAnswer.Length >= question.Minimum &&
               dto.TextAnswer.Length <= question.Maximum;

        if (dto.TextAnswer is null)
            return Result.Failure($"Field TextAnswer of answer {dto.Number} is null");

        if (dto.TextAnswer.Length > question.Maximum || dto.TextAnswer.Length < question.Minimum)
            return Result.Failure($"Text length out of range in answer {dto.Number}");

        return Result.Success();
    }
}
