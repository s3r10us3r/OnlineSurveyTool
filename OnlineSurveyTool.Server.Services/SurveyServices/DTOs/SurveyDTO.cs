using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SurveyDTO
{
    [StringLength(36)]
    public string Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
    
    public DateTime? OpeningDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    [Required]
    public bool IsOpen { get; set; }
    
    [Required]
    public List<QuestionBase> Questions { get; set; }
}