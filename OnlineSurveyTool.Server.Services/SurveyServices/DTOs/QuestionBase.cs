using System.ComponentModel.DataAnnotations;
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
    [Required]
    public int Number { get; init; }
    [Required]
    public string Value { get; init; }
    [Required]
    public bool CanBeSkipped { get; init; }
}
