using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.SurveyService.DTOs;

public class SurveyEditDto
{
    [Required] 
    [StringLength(36, MinimumLength = 36)]
    public string Id { get; set; }
    
    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; set; }
    
    public List<QuestionDTO>? NewQuestions { get; set; }
    
    public List<QuestionEditDto>? EditedQuestions { get; set; }
    
    //this list contains ids only
    public List<string>? DeletedQuestions { get; set; }
}