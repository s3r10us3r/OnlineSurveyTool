using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class SurveyDTO
{
    [StringLength(36, MinimumLength = 36)]
    public string? Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }
   
    [Required]
    [MinLength(1)]
    public List<QuestionDTO> Questions { get; set; }
}