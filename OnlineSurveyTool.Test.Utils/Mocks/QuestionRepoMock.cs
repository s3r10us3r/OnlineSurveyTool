using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class QuestionRepoMock : BaseMock<Question, string>, IQuestionRepo
{
    public QuestionRepoMock(IPopulator<Question, string> populator) : base(populator)
    {
    }

    public async Task<Question> LoadAnswers(Question entity)
    {
        throw new NotImplementedException();
    }
}