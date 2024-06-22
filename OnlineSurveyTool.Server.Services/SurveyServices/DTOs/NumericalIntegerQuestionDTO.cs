using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class NumericalIntegerQuestionDTO: QuestionBase
{
    public NumericalIntegerQuestionDTO()
    {
    }
    
    public NumericalIntegerQuestionDTO(Question question) : base(question)
    {
        if (question.Minimum is null || question.Maximum is null)
        {
            ThrowArgumentNullError(question.Id);
        }

        MinimalAnswer = (int)question.Minimum!;
        MaximalAnswer = (int)question.Maximum!;
    }

    public override Question ToQuestion()
    {
        var question = base.ToQuestion();
        question.Maximum = MaximalAnswer;
        question.Minimum = MinimalAnswer;
        return question;
    }

    public int MinimalAnswer { get; }
    public int MaximalAnswer { get; }
}