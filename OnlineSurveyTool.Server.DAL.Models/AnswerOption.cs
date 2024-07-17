using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class AnswerOption : EntityBaseIntegerId
    {
        [ForeignKey(nameof(Answer))]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        [ForeignKey(nameof(ChoiceOption))]
        public string ChoiceOptionId { get; set; }
        public ChoiceOption ChoiceOption { get; set; }
    }
}
