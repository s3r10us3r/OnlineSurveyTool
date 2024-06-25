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
    public int Number { get; set; }
    [Required]
    [StringLength(200)]
    public string Value { get; set; }
    [Required]
    public bool CanBeSkipped { get; set; }
}
