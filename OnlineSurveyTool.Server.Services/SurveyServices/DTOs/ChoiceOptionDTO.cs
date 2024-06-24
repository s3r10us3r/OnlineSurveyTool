using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class ChoiceOptionDTO
{
    [Required]
    [StringLength(300)]
    public string Value { get; init; }
    [Required]
    public int Number { get; init; }
}