using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class EditSurveyValidator : IEditSurveyValidator
{
    private readonly IQuestionValidator _questionValidator;
    private readonly ISurveyValidator _surveyValidator;
    private readonly ISurveyConverter _surveyConverter;

    public EditSurveyValidator(IQuestionValidator questionValidator, ISurveyConverter surveyConverter,
        ISurveyValidator surveyValidator)
    {
        _questionValidator = questionValidator;
        _surveyConverter = surveyConverter;
        _surveyValidator = surveyValidator;
    }
    
    public bool ValidateSurveyEdit(SurveyEditDto edit, Survey survey, out string message)
    {
        var surveyDto = _surveyConverter.SurveyToDto(survey);
        if (edit.DeletedQuestions is not null)
        {
            if (!ValidateDeletedQuestions(survey.Questions, edit.DeletedQuestions))
            {
                message = "One or more questions from deleted questions does not exist in the survey.";
                return false;
            }

            surveyDto.Questions.RemoveAll(q => edit.DeletedQuestions.Contains(q.Id!));
        }

        if (edit.EditedQuestions is not null)
        {
            if (!ValidateEditQuestions(survey.Questions, edit.EditedQuestions))
            {
                message = "One or more questions from edited questions does not exist in the survey.";
            }
            edit.EditedQuestions.ForEach(e => MapEditToQuestionDto(e, surveyDto.Questions.Find(q => q.Id == e.Id)!));
        }

        if (edit.NewQuestions is not null)
        {
            var newMessage = "";
            var newQuestionsValid = edit.NewQuestions.All(q => _questionValidator.ValidateQuestion(q, out newMessage));
            if (!newQuestionsValid)
            {
                message = $"One of questions in newQuestionsValid is not valid message: {newMessage}";
            }
            surveyDto.Questions.AddRange(edit.NewQuestions);
        }

        var validationResult = _surveyValidator.ValidateSurvey(surveyDto, out var vMessage);
        if (!validationResult)
        {
            message = vMessage;
            return false;
        }

        message = "";
        return true;
    }
    
    private bool ValidateDeletedQuestions(IEnumerable<Question> questions, List<string> toDelete)
    {
        return toDelete.Select(deleteId => questions.FirstOrDefault(q => q.Id == deleteId))
            .All(qToDelete => qToDelete is not null);
    }

    private bool ValidateEditQuestions(IEnumerable<Question> questions, List<QuestionEditDto> edits)
    {
        return edits.Select(e => questions.FirstOrDefault(q => q.Id == e.Id))
            .All(qToEdit => qToEdit is not null);
    }

    private void MapEditToQuestionDto(QuestionEditDto edit, QuestionDTO question)
    {
        question.Number = edit.Number ?? question.Number;
        question.Value = edit.Value ?? question.Value;
        question.Minimum = edit.Minimum ?? question.Minimum;
        question.Maximum = edit.Maximum ?? question.Maximum;
        question.CanBeSkipped = edit.CanBeSkipped ?? question.CanBeSkipped;
        question.ChoiceOptions = edit.ChoiceOptions ?? question.ChoiceOptions;
    }
}