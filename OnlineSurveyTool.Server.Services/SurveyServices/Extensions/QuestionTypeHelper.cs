using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Extensions;

public static class QuestionTypeHelper
{
    public static string ToStringFriendly(this QuestionType type)
    {
        return type switch
        {
            QuestionType.SingleChoice => "Single Choice",
            QuestionType.MultipleChoice => "Multiple Choice",
            QuestionType.NumericalInteger => "Numerical Integer",
            QuestionType.NumericalDouble => "Numerical Double",
            QuestionType.Textual => "Textual",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static QuestionType? FromString(string typeString)
    {
        return typeString switch
        {
            "Single Choice" => QuestionType.SingleChoice,
            "Multiple Choice" => QuestionType.MultipleChoice,
            "Numerical Double" => QuestionType.NumericalDouble,
            "Numerical Integer" => QuestionType.NumericalInteger,
            "Textual" => QuestionType.Textual,
            _ => null
        };
    }
}