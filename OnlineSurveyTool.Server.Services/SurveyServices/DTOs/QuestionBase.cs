using System.Text.Json.Serialization;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

[JsonDerivedType(typeof(MultipleChoiceQuestionDTO))]
[JsonDerivedType(typeof(SingleChoiceQuestionDTO))]
[JsonDerivedType(typeof(NumericalDoubleQuestionDTO))]
[JsonDerivedType(typeof(NumericalIntegerQuestionDTO))]
[JsonDerivedType(typeof(TextualQuestionDTO))]
public abstract class QuestionBase
{
    public string Value { get; }
    public bool CanBeSkipped { get; }

    public QuestionBase()
    {
    }

    public virtual Question ToQuestion()
    {
        return new Question()
        {
            Value = Value,
            CanBeSkipped = CanBeSkipped
        };
    }

    protected QuestionBase(Question question)
    {
        Value = question.Value;
        CanBeSkipped = question.CanBeSkipped;
    }

    protected void ThrowArgumentNullError(int questionId)
    {
        throw new ArgumentNullException($"Question {questionId} does not include relevant information (maybe it was marked with a wrong type?)");
    }
}