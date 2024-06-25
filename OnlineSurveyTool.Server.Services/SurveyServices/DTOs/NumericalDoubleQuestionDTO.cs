using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class NumericalDoubleQuestionDTO : QuestionBase
{
    [Required]
    public double MinimalAnswer { get; set; }
    [Required]
    public double MaximalAnswer { get; set; }
}