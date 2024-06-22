using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class TextualQuestionDTO: QuestionBase
{
    public TextualQuestionDTO()
    {
    }
    
    public TextualQuestionDTO(Question question) : base(question)
    {
        if (question.Minimum is null || question.Maximum is null)
        {
            ThrowArgumentNullError(question.Id);
        }

        MinimalLength = (int)question.Minimum!;
        MaximalLength = (int)question.Maximum!;
    }

    public override Question ToQuestion()
    {
        var question  = base.ToQuestion();
        question.Maximum = MaximalLength;
        question.Minimum = MinimalLength;
        return question;
    }

    public int MinimalLength { get; set; }
    public int MaximalLength { get; set; }
}