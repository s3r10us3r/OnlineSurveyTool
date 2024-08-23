using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    //whoever stumbles upon this code must know that it is very stupid, both questions and answers should have been 
    //split up to different tables based on type
    public class Answer : EntityBaseIntegerId
    {
        public QuestionType Type { get; set; }

        [ForeignKey(nameof(Question))]
        public string QuestionId { get; set; }

        [ForeignKey(nameof(SurveyResultId))]
        public int SurveyResultId { get; set; }

        [ForeignKey(nameof(SingleChoiceOption))]
        public string? SingleChoiceOptionId { get; set; }

        public virtual SurveyResult? SurveyResult { get; set; }
        public virtual ChoiceOption? SingleChoiceOption { get; set; }
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
        public virtual Question Question { get; set; }


        [NotMapped]
        public ICollection<ChoiceOption>? ChoiceOptions => AnswerOptions?.Select(ao => ao.ChoiceOption).ToList();
    }
}
