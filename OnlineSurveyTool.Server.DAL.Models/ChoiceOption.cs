using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Answers")]
    [Index(nameof(QuestionId))]
    public class ChoiceOption : EntityBase
    {
        //Order of the answer inside a question
        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(36)]
        public string ExternalId { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Value { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public virtual IEnumerable<Answer> Answers { get; set; }
        public virtual IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
}
