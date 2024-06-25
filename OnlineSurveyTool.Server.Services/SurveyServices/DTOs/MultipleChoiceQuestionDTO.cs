using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public sealed class MultipleChoiceQuestionDTO : QuestionBase
{
    [Required]
    public int MinimalChoices { get; set; }
    [Required]
    public int MaximalChoices { get; set; }
    [Required]
    public List<ChoiceOptionDTO> ChoiceOptions { get; set; }
}