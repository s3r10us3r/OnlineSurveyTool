using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.AnswerServices.DTOs;

public class AnswerDTO
{
    [Required]
    public int Number { get; set; }
    
    public List<int>? ChosenOptions { get; set; }
    
    public double? Answer { get; set; }
    
    public string? TextAnswer { get; set; }
}
