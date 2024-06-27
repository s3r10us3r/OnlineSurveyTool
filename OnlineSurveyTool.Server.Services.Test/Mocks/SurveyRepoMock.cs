using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class SurveyRepoMock: BaseMock<Survey>, ISurveyRepo
{
    protected override void Populate()
    {
        Entities.Add(
            new Survey()
            {
                Id = 1,
                OpeningDate = DateTime.Today,
                ClosingDate = DateTime.Today.AddDays(1),
                IsArchived = false,
                IsOpen = true,
                Name = "Survey",
                Token = "test",
                Questions = [
                    new Question()
                    {
                        Type = QuestionType.SingleChoice,
                        CanBeSkipped = false,
                        ChoiceOptions = [new ChoiceOption {Number = 1, Value = "Choice 1"}, new ChoiceOption {Number = 2, Value = "Choice 2"}],
                        Number = 1,
                        Value = "Question 1"
                    },
                    new Question()
                    {
                        Type = QuestionType.MultipleChoice,
                        CanBeSkipped = false,
                        ChoiceOptions = [new ChoiceOption {Number = 1, Value = "Choice 1"}, new ChoiceOption {Number = 2, Value = "Choice 2"}],
                        Number = 2,
                        Value = "Question 2",
                        Minimum = 1,
                        Maximum = 2
                    },
                    new Question()
                    {
                        Type = QuestionType.NumericalDouble,
                        CanBeSkipped = false,
                        Number = 3,
                        Minimum = 1,
                        Maximum = 10,
                        Value = "Question 3"
                    },
                    new Question()
                    {
                        Type = QuestionType.NumericalInteger,
                        CanBeSkipped = false,
                        Number = 4,
                        Minimum = 1,
                        Maximum = 10,
                        Value = "Question 4"
                    },
                    new Question()
                    {
                        Type = QuestionType.Textual,
                        CanBeSkipped = false,
                        Number = 5,
                        Minimum = 10,
                        Maximum = 1000,
                        Value = "Question 5"
                    }
                ]
            });
    }

    public async Task<Survey?> GetOne(string token)
    {
        return Entities.FirstOrDefault(s => s.Token == token);
    }

    public async Task<List<Survey>> GetAllSurveysOwnedBy(int userId)
    {
        return Entities.Where(s => s.IsOpen && s.OwnerId == userId).ToList();
    }
}