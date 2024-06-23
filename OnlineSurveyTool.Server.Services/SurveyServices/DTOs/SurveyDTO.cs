using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SurveyDTO
{
    [Required]
    [StringLength(256, MinimumLength = 256)]
    public string Token { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
    
    public DateTime? OpeningDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    [Required]
    public bool IsOpen { get; set; }
    
    [Required]
    public IEnumerable<QuestionBase> Questions { get; set; }
}