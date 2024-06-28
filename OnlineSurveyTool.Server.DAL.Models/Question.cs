using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Questions")]
    [Index(nameof(SurveyId))]
    [Index(nameof(ExternalId))]
    public class Question : EntityBase
    {
        //Order inside survey
        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(36)]
        public string ExternalId { get; set; }
        
        [Required]
        public string Value { get; set; }

        [Required]
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }

        public QuestionType Type { get; set; }

        //If Type is SingleChoice these properties do not mean anything
        //If Type is MultipleChoice these properties mean minimal and maximal number of choices a user can mark
        //If Type is NumericalInteger or NumericalDouble then these specify minimal and maximal value of an answer
        //If Type is Textual these specify minimal and maximal length of an answer string
        public double? Minimum { get; set; }

        public double? Maximum { get; set; }

        [Required]
        public bool CanBeSkipped { get; set; }

        //This is null for Numerical and Textual type of questions
        public virtual IEnumerable<ChoiceOption>? ChoiceOptions { get; set; }

        public virtual IEnumerable<Answer> Answers { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
