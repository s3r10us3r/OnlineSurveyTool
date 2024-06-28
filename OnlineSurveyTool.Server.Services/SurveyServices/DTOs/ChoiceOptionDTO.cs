using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class ChoiceOptionDTO
{
    [Required]
    [StringLength(300)]
    public string Value { get; set; }
    
    [StringLength(36)]
    public string? Id { get; set; }
    
    [Required]
    public int Number { get; set; }
}