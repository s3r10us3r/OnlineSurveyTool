using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.StatServices.DTOs;

public class SurveyHeaderDto
{
    [Required]
    [StringLength(36, MinimumLength = 36)]
    public string Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }
    
    [Required]
    public bool IsOpen { get; set; }
    
    [Required]
    public int ResultCount { get; set; }
}