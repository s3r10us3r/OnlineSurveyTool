using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks.Populators;

public class ChoiceOptionPopulator : IPopulator<ChoiceOption, string>
{
    public List<ChoiceOption> Populate()
    {
        return
        [
            new() {Id = "q1co1", Number = 0, QuestionId = "question1", Value = "Option 1"},
            new() {Id = "q1co2", Number = 1, QuestionId = "question1", Value = "Option 2"},
            new() {Id = "q1co3", Number = 2, QuestionId = "question1", Value = "Option 3"},
            new() {Id = "q2co1", Number = 0, QuestionId = "question2", Value = "Option 1"},
            new() {Id = "q2co2", Number = 1, QuestionId = "question2", Value = "Option 2"},
            new() {Id = "q2co3", Number = 2, QuestionId = "question2", Value = "Option 3"},
        ];
    }
}