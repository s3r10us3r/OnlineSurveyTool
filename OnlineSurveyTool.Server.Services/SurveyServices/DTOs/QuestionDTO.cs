using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class QuestionDTO
{
    [StringLength(36, MinimumLength = 36)]
    public string? Id { get; set; }
    
    [Required]
    public int Number { get; set; }
    
    [Required]
    public string Value { get; set; }
    
    [Required]
    public string Type { get; set; }

    [Required]
    public bool CanBeSkipped { get; set; }
    
    public double? Minimum { get; set; }
    
    public double? Maximum { get; set; }
    
    public List<ChoiceOptionDTO>? ChoiceOptions { get; set; }
}