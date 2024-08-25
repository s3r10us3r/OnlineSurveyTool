using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [PrimaryKey(nameof(SurveyResultId), nameof(QuestionNumber))]
    public abstract class Answer
    {
        [ForeignKey(nameof(Result))]
        public int SurveyResultId { get; set; }
        
        public int QuestionNumber { get; set; }
        
        public SurveyResult Result { get; set; }
    }
}
