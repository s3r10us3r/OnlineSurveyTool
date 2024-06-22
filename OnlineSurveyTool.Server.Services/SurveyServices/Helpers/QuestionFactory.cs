using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

public class QuestionFactory: IQuestionFactory
{
    public QuestionBase MakeQuestion(Question question)
    {
        return question.Type switch
        {
            QuestionType.SingleChoice => new SingleChoiceQuestionDTO(question),
            QuestionType.MultipleChoice => new MultipleChoiceQuestionDTO(question),
            QuestionType.NumericalInteger => new NumericalIntegerQuestionDTO(question),
            QuestionType.NumericalDouble => new NumericalDoubleQuestionDTO(question),
            QuestionType.Textual => new TextualQuestionDTO(question),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}