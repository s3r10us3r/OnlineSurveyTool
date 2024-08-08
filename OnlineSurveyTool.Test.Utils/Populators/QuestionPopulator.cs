using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Test.Utils.Populators;

public class QuestionPopulator : IPopulator<Question, string>
{
    public List<Question> Populate()
    {
        List<Question> questions =
        [
            new Question()
            {
                Id = "question1",
                SurveyId = "survey1",
                Type = QuestionType.SingleChoice,
                CanBeSkipped = false,
                Value = "Question 1",
                ChoiceOptions = [
                    new() {Id = "q1co1", Number = 0, QuestionId = "question1", Value = "Option 1"},
                    new() {Id = "q1co2", Number = 1, QuestionId = "question1", Value = "Option 2"},
                    new() {Id = "q1co3", Number = 2, QuestionId = "question1", Value = "Option 3"},
                ],
                Number = 0
            },
            
            new Question()
            {
                Id = "question2",
                SurveyId = "survey1",
                CanBeSkipped = false,
                Type = QuestionType.MultipleChoice,
                Value = "Question 2",
                ChoiceOptions = [
                    new() {Id = "q2co1", Number = 0, QuestionId = "question2", Value = "Option 1"},
                    new() {Id = "q2co2", Number = 1, QuestionId = "question2", Value = "Option 2"},
                    new() {Id = "q2co3", Number = 2, QuestionId = "question2", Value = "Option 3"},
                ],
                Number = 1,
                Minimum = 1,
                Maximum = 2,
            },
            
            new Question()
            {
                Id = "question3",
                SurveyId = "survey1",
                Type = QuestionType.NumericalDouble,
                Value = "Question 3",
                CanBeSkipped = false,
                Number = 2,
                Maximum = 10.5,
                Minimum = -13.7,
            },
            
            new Question()
            {
                Id = "question4",
                SurveyId = "survey1",
                Type = QuestionType.NumericalInteger,
                Value = "Question 4",
                CanBeSkipped = false,
                Number = 3,
                Maximum = 10,
                Minimum = -10,
            },
            
            new Question()
            {
                Id = "question5",
                SurveyId = "survey1",
                Type = QuestionType.NumericalDouble,
                Value = "Question 5",
                CanBeSkipped = false,
                Number = 4,
                Maximum = 100,
                Minimum = 1,
            },
        ];

        return questions;
    }
}