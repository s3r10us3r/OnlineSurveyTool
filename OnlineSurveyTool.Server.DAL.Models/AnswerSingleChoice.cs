using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models;

public class AnswerSingleChoice : Answer
{
    [ForeignKey(nameof(ChoiceOption))]
    public string ChoiceOptionId { get; set; }
    
    public ChoiceOption ChoiceOption { get; set; }
}