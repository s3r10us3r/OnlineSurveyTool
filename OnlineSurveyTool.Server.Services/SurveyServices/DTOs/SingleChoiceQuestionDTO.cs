using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SingleChoiceQuestionDTO: QuestionBase
{
    [Required]
    public List<ChoiceOptionDTO> ChoiceOptions { get; init; }
}