using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DAL.Models;

public class AnswerTextual : Answer
{
    [StringLength(10000)]
    public string Text { get; set; }
}