using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class NumericalIntegerQuestionDTO: QuestionBase
{
    [Required]
    public int MinimalAnswer { get; set; }
    [Required]
    public int MaximalAnswer { get; set; }
}