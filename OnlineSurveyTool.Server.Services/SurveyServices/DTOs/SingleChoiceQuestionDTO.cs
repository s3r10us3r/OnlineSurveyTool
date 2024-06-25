using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SingleChoiceQuestionDTO: QuestionBase
{
    [Required]
    public List<ChoiceOptionDTO> ChoiceOptions { get; set; }
}