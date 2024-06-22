using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public sealed class MultipleChoiceQuestionDTO : QuestionBase
{
    public MultipleChoiceQuestionDTO()
    {
    }
    public MultipleChoiceQuestionDTO(Question question) : base(question)
    {
        if (question.ChoiceOptions is null || question.Minimum is null || question.Maximum is null)
        {
            ThrowArgumentNullError(question.Id);
        }
        ChoiceOptions = question.ChoiceOptions!.Select(c => new ChoiceOptionDTO(c)).ToList();
        MinimalChoices = (int)question.Minimum!;
        MaximalChoices = (int)question.Maximum!;
    }

    public override Question ToQuestion()
    { 
        var question = base.ToQuestion();
        question.Minimum = MinimalChoices;
        question.Maximum = MaximalChoices;
        question.ChoiceOptions = ChoiceOptions.Select(c => c.ToChoiceOption());
        return question;
    }

    public int MinimalChoices { get; }
    public int MaximalChoices { get; }
    public List<ChoiceOptionDTO> ChoiceOptions { get; }
}