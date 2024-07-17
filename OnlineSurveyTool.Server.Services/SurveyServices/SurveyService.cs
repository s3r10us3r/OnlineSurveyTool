using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Extensions;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices;

public class SurveyService : ISurveyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISurveyValidator _surveyValidator;
    private readonly IQuestionRepo _questionRepo;
    private readonly ISurveyRepo _surveyRepo;
    private readonly IUserRepo _userRepo;
    private readonly IChoiceOptionRepo _choiceOptionRepo;
    private readonly IEditSurveyValidator _editSurveyValidator;
    private readonly ILogger<SurveyService> _logger;

    public SurveyService(IUnitOfWork unitOfWork, ISurveyValidator surveyValidator,
        IEditSurveyValidator editSurveyValidator, ILogger<SurveyService> logger)
    {
        _unitOfWork = unitOfWork;
        _surveyValidator = surveyValidator;
        _questionRepo = _unitOfWork.QuestionRepo;
        _surveyRepo = _unitOfWork.SurveyRepo;
        _userRepo = _unitOfWork.UserRepo;
        _choiceOptionRepo = _unitOfWork.ChoiceOptionRepo;
        _editSurveyValidator = editSurveyValidator;
        _logger = logger;
    }

    public async Task<IResult<SurveyDTO>> GetSurvey(string id)
    {
        var survey = await _surveyRepo.GetOne(id);
        if (survey is null || !survey.IsOpen)
        {
            return Result<SurveyDTO>.Failure("Survey not found");
        }

        var dto = survey.SurveyToDto();
        return Result<SurveyDTO>.Success(dto);
    }
    
    public async Task<IResult<SurveyDTO>> AddSurvey(string ownerLogin, SurveyDTO surveyDto)
    {
        if (!_surveyValidator.ValidateSurvey(surveyDto, out var message))
        {
            _logger.LogWarning("Invalid survey supplied to SurveyService.AddSurvey error message: {message}", message);
            return Result<SurveyDTO>.Failure(message);
        }

        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            _logger.LogError("Nonexistent user login {login} supplied to SurveyService.AddSurvey", ownerLogin);
            return Result<SurveyDTO>.Failure("User with this login does not exist.");
        }

        var survey = surveyDto.DtoToSurvey();
        survey.OwnerId = user.Id;
        await _surveyRepo.Add(survey);
        var resultDto = survey.SurveyToDto();
        return Result<SurveyDTO>.Success(resultDto);
    }
    
    public async Task<IResult<SurveyDTO, SurveyServiceFailureReason>> EditSurvey(string ownerLogin, SurveyEditDto editedSurvey)
    {
        Console.WriteLine(editedSurvey.Id);
        var surveys = await _surveyRepo.GetAll();
        var survey = await _surveyRepo.GetOne(editedSurvey.Id);
        if (survey is null)
        {
            _logger.LogWarning("Nonexistent survey with id {id} supplied to EditSurvey", editedSurvey.Id);
            return Result<SurveyDTO, SurveyServiceFailureReason>.Failure("SurveyId does not exist",
                SurveyServiceFailureReason.DoesNotExist);
        }

        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            //if this is triggered it means that somehow authentication was bypassed
            _logger.LogCritical("Nonexistent user login {login} supplied to SurveyService.EditSurvey", ownerLogin);
            throw new ArgumentException($"User with login {ownerLogin} does not exist");
        }

        if (user.Id != survey.OwnerId)
        {
            _logger.LogWarning("Survey {sId} with owner {oId} was tried to be edited by user {uId}", survey.Id,
                survey.OwnerId, user.Id);
            return Result<SurveyDTO, SurveyServiceFailureReason>.Failure("User does not have access to this survey.",
                SurveyServiceFailureReason.NotAuthorized);
        }


        var isValid = _editSurveyValidator.ValidateSurveyEdit(editedSurvey, survey, out var validationMessage);
        if (!isValid)
        {
            _logger.LogWarning("Invalid edit model sent to SurveyService.EditSurvey, message {e}", validationMessage);
            return Result<SurveyDTO, SurveyServiceFailureReason>.Failure(validationMessage,
                SurveyServiceFailureReason.InvalidRequest);
        }

        //from now on I assume that everything is valid
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            if (editedSurvey.Name is not null)
            {
                survey.Name = editedSurvey.Name;
                await _surveyRepo.Update(survey);
            }

            await Task.WhenAll(editedSurvey.DeletedQuestions!.Select(id => _questionRepo.Remove(id)));
            if (editedSurvey.EditedQuestions is not null)
            {
                var pairs = editedSurvey.EditedQuestions.Join(survey.Questions, e => e.Id, q => q.Id,
                    (e, q) => (q, e));

                foreach (var (q, e) in pairs)
                {
                    await MapEditToQuestion(q, e);
                    await _questionRepo.Update(q);
                }
            }

            if (editedSurvey.NewQuestions is not null)
            {
                var questions = editedSurvey.NewQuestions
                    .Select(q => q.DtoToQuestion())
                    .ToList();
                questions.ForEach(q => q.SurveyId = survey.Id);
                await _questionRepo.AddRange(questions);
            }

            await _unitOfWork.CommitTransactionAsync();
            var finalSurvey = await _surveyRepo.GetOne(survey.Id);
            var finalSurveyDto = finalSurvey!.SurveyToDto();
            return Result<SurveyDTO, SurveyServiceFailureReason>.Success(finalSurveyDto);
        }
        catch (Exception e)
        {
            _logger.LogError("Error in EditSurvey {e}", e);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    
    public async Task<IResult> DeleteSurvey(string ownerLogin, string surveyId)
    {
        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            _logger.LogCritical("User with login {oL} supplied to SurveyService.DeleteSurvey", ownerLogin);
            throw new ArgumentException();
        }
        var survey = await _surveyRepo.GetOne(surveyId);
        if (survey is null)
        {
            _logger.LogWarning("Nonexistent survey {sId} supplied to SurveyService.DeleteSurvey", surveyId);
            return Result.Failure("Survey does not exist.");
        }

        if (survey.OwnerId != user.Id)
        {
            _logger.LogWarning("User {ownerLogin} tried to delete a survey that they do not won {surveyId}",
                ownerLogin, surveyId);
            return Result.Failure("User does not have access to this survey.");
        }
        
        await _surveyRepo.Remove(surveyId);
        return Result.Success();
    }

    public async Task<IResult> OpenSurvey(string ownerLogin, string surveyId)
    {
        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            _logger.LogCritical("User with login {oL} supplied to SurveyService.CloseSurvey", ownerLogin);
            throw new ArgumentException();
        }
        var survey = await _surveyRepo.GetOne(surveyId);
        if (survey is null)
        {
            _logger.LogWarning("Nonexistent survey {sId} supplied to SurveyService.CloseSurvey", surveyId);
            return Result.Failure("Survey does not exist.");
        }

        if (survey.OwnerId != user.Id)
        {
            _logger.LogWarning("User {ownerLogin} tried to open a survey that they do not own {surveyId}", ownerLogin,
                surveyId);
            return Result.Failure("User does not have access to this survey.");
        }

        survey.IsOpen = true;
        await _surveyRepo.Update(survey);
        return Result.Success();
    }

    public async Task<IResult> CloseSurvey(string ownerLogin, string surveyId)
    { 
        var user = await _userRepo.GetOne(ownerLogin);
        if (user is null)
        {
            _logger.LogCritical("User with login {oL} supplied to SurveyService.CloseSurvey", ownerLogin);
            throw new ArgumentException();
        }
        var survey = await _surveyRepo.GetOne(surveyId);
        if (survey is null)
        {
            _logger.LogWarning("Nonexistent survey {sId} supplied to SurveyService.CloseSurvey", surveyId);
            return Result.Failure("Survey does not exist.");
        }
 
        if (survey.OwnerId != user.Id)
        {
            _logger.LogWarning("User {ownerLogin} tried to close a survey that they do not own {surveyId}", ownerLogin,
                surveyId);
            return Result.Failure("User does not have access to this survey.");
        }
 
        survey.IsOpen = false;
        await _surveyRepo.Update(survey);
        return Result.Success();       
    }

    private async Task MapEditToQuestion(Question question, QuestionEditDto dto)
    {
        question.Number = dto.Number ?? question.Number;
        question.Minimum = dto.Minimum ?? question.Minimum;
        question.Maximum = dto.Maximum ?? question.Maximum;
        question.Value = dto.Value ?? question.Value;
        question.CanBeSkipped = dto.CanBeSkipped ?? question.CanBeSkipped;

        if (dto.ChoiceOptions is not null)
        {
            await Task.WhenAll(question.ChoiceOptions!.Select(co => _choiceOptionRepo!.Remove(co)));
            var convertedChoiceOptions = dto.ChoiceOptions.Select(co => co.DtoToChoiceOption());
            await _choiceOptionRepo.AddRange(convertedChoiceOptions.ToList());
        }
    }
}