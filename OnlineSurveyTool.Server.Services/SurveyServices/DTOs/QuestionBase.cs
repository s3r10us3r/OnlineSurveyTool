using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

[JsonDerivedType(typeof(MultipleChoiceQuestionDTO))]
[JsonDerivedType(typeof(SingleChoiceQuestionDTO))]
[JsonDerivedType(typeof(NumericalDoubleQuestionDTO))]
[JsonDerivedType(typeof(NumericalIntegerQuestionDTO))]
[JsonDerivedType(typeof(TextualQuestionDTO))]
public abstract class QuestionBase
{
    [Required]
    public int Number { get; init; }
    [Required]
    [StringLength(200)]
    public string Value { get; init; }
    [Required]
    public bool CanBeSkipped { get; init; }
}
