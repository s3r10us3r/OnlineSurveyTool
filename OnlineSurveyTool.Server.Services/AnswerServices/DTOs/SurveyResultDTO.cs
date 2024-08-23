using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.AnswerServices.DTOs;

public class SurveyResultDTO
{
    [Required] [StringLength(36, MinimumLength = 36)]
    public string Id { get; set; }

    [Required]
    public List<AnswerDTO> Answers { get; set; }
}