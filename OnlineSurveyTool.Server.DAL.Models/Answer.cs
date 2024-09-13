using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [PrimaryKey(nameof(SurveyResultId), nameof(QuestionNumber))]
    public class Answer
    {
        [ForeignKey("Result")]
        [StringLength(36, MinimumLength = 36)]
        public string SurveyResultId { get; set; }
        
        public int QuestionNumber { get; set; }
        
        public SurveyResult Result { get; set; }

        public AnswerDiscriminator Discriminator { get; set; }
        
        [StringLength(36, MinimumLength = 36)]
        [ForeignKey(nameof(ChoiceOption))]
        public string? ChoiceOptionId { get; set; }
        
        public ChoiceOption? ChoiceOption { get; set; }
        
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
        
        public double? NumericalAnswer { get; set; }
        
        [StringLength(10000)]
        public string? TextAnswer { get; set; }
    }

    public enum AnswerDiscriminator
    {
        SingleChoice, MultipleChoice, Numerical, Textual
    }
}
