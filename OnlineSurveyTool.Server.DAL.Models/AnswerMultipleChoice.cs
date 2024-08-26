using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models;

public class AnswerMultipleChoice : Answer
{
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
    
    [NotMapped]
    public List<ChoiceOption> ChoiceOptions { get; set; }
}