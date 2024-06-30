using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class QuestionDTO
{
    [Required]
    public int Number { get; set; }
    
    [Required]
    [StringLength(36)]
    public string ExternalId { get; set; }
}