using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class ChoiceOptionDTO
{
    [Required]
    public int Number { get; set; }
    
    [Required]
    [StringLength(36)]
    public string? Id { get; set; }
    
    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Value { get; set; }
}