using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public sealed class MultipleChoiceQuestionDTO : QuestionBase
{
    [Required]
    public int MinimalChoices { get; init; }
    [Required]
    public int MaximalChoices { get; init; }
    [Required]
    public List<ChoiceOptionDTO> ChoiceOptions { get; init; }
}