using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SingleChoiceQuestionDTO: QuestionBase
{
    public SingleChoiceQuestionDTO()
    {
    }
    
    public SingleChoiceQuestionDTO(Question question) : base(question)
    {
        if (question.ChoiceOptions is null)
        {
            ThrowArgumentNullError(question.Id);
        }
        
        ChoiceOptions = question.ChoiceOptions!.Select(c => new ChoiceOptionDTO(c)).ToList();
    }

    public override Question ToQuestion()
    {
        var question = base.ToQuestion();
        question.ChoiceOptions = ChoiceOptions.Select(c => c.ToChoiceOption());
        return question;
    }

    public List<ChoiceOptionDTO> ChoiceOptions { get; }
}