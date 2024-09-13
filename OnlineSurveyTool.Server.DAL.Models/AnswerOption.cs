using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class AnswerOption : EntityBaseIntegerId
    {
        public string ResultId { get; set; }
        
        public int Number { get; set; }
        
        [ForeignKey($"{nameof(ResultId)},{nameof(Number)}")]
        public Answer Answer { get; set; }

        [ForeignKey(nameof(ChoiceOption))]
        public string ChoiceOptionId { get; set; }
        public ChoiceOption ChoiceOption { get; set; }
    }
}
