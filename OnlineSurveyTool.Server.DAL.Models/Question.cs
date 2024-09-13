using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Questions")]
    public class Question : EntityBaseStringId
    {
        //Order inside survey
        [Required]
        public int Number { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        [ForeignKey("Survey")]
        public string SurveyId { get; set; }

        [Required]
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
        public virtual ICollection<ChoiceOption> ChoiceOptions { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
