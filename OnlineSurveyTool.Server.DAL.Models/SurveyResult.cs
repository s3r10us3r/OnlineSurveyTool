using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Results")]
    public class SurveyResult : EntityBaseIntegerId
    {
        [ForeignKey("Survey")] public string SurveyId;

        public DateTime TimeStamp { get; set; }

        public virtual Survey Survey { get; set; }

        public ICollection<AnswerSingleChoice> SingleChoiceAnswers { get; set; } = [];
        public ICollection<AnswerMultipleChoice> MultipleChoiceAnswers { get; set; } = [];
        public ICollection<AnswerNumerical> NumericalAnswers { get; set; } = [];
        public ICollection<AnswerTextual> TextualAnswers { get; set; } = [];

        [NotMapped]
        public ICollection<Answer> Answers
        {
            get => GetAnswers();
            set => SetAnswers(value);
        }

        private List<Answer> GetAnswers()
        {
            List<Answer> answers = [];
            answers.AddRange(SingleChoiceAnswers);
            answers.AddRange(MultipleChoiceAnswers);
            answers.AddRange(NumericalAnswers);
            answers.AddRange(TextualAnswers);
            return answers;
        }

        private void SetAnswers(ICollection<Answer> answers)
        {
            foreach (var answer in answers) 
                AddToAnswersBasedOnType(answer);
        }

        private void AddToAnswersBasedOnType(Answer answer)
        {
            switch (answer)
            {
                case AnswerSingleChoice ans:
                    SingleChoiceAnswers.Add(ans);
                    return;
                case AnswerMultipleChoice ans:
                    MultipleChoiceAnswers.Add(ans);
                    return;
                case AnswerTextual ans:
                    TextualAnswers.Add(ans);
                    return;
                case AnswerNumerical ans:
                    NumericalAnswers.Add(ans);
                    return;
            }
        }
    }
}
