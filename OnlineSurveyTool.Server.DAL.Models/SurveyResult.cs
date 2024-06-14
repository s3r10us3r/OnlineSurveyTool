using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Results")]
    public class SurveyResult : EntityBase
    {
        [ForeignKey("Survey")]
        public int SurveyId;

        [Required]
        public DateTime TimeStamp { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual IEnumerable<Answer> Answers { get; set; }
    }
}
