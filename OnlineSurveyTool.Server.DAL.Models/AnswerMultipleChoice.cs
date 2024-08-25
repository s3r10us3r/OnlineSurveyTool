namespace OnlineSurveyTool.Server.DAL.Models;

public class AnswerMultipleChoice : Answer
{
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
}