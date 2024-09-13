using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Results")]
    public class SurveyResult : EntityBaseStringId
    {
        [ForeignKey("Survey")] public string SurveyId;

        public DateTime TimeStamp { get; set; }

        public virtual Survey Survey { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
