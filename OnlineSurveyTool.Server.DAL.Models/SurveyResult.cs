using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Results")]
    public class SurveyResult : EntityBaseIntegerId
    {
        [ForeignKey("Survey")]
        public string SurveyId;

        [Required]
        public DateTime TimeStamp { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual IEnumerable<Answer> Answers { get; set; }
    }
}
