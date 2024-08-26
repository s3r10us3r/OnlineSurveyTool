using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public class AnswerOption : EntityBaseIntegerId
    {
        public int ResultId { get; set; }
        
        public int Number { get; set; }
        
        [ForeignKey($"${nameof(ResultId)},{nameof(Number)}")]
        public AnswerMultipleChoice Answer { get; set; }

        [ForeignKey(nameof(ChoiceOption))]
        public string ChoiceOptionId { get; set; }
        public ChoiceOption ChoiceOption { get; set; }
    }
}
