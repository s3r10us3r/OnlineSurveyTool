using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class QuestionEditDto
{
    [Required]
    [StringLength(36, MinimumLength = 36)]
    public string Id { get; set; }
    
    public int? Number { get; set; }
    
    public string? Value { get; set; }
    
    public double? Minimum { get; set; }
    
    public double? Maximum { get; set; }
    
    public bool? CanBeSkipped { get; set; }
    
    public List<ChoiceOptionDTO>? ChoiceOptions { get; set; }
}