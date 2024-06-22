using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class NumericalDoubleQuestionDTO : QuestionBase
{
    public NumericalDoubleQuestionDTO()
    {
    }
    
    public NumericalDoubleQuestionDTO(Question question) : base(question)
    {
        if (question.Minimum is null || question.Maximum is null)
        {
            ThrowArgumentNullError(question.Id);
        }

        MinimalAnswer = (double)question.Minimum!;
        MaximalAnswer = (double)question.Maximum!;
    }

    public override Question ToQuestion()
    {
        var question = base.ToQuestion();
        question.Maximum = MaximalAnswer;
        question.Minimum = MinimalAnswer;
        return question;
    }

    public double MinimalAnswer { get; }
    public double MaximalAnswer { get; }
}