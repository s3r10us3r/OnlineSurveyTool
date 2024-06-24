using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class TextualQuestionDTO : QuestionBase
{
    [Required]
    public int MinimalLength { get; init; }
    [Required]
    [Range(1, 1000)]
    public int MaximalLength { get; init; }
}