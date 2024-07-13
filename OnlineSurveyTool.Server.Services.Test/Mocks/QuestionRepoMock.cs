using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class QuestionRepoMock : BaseMock<Question, string>, IQuestionRepo
{
    public QuestionRepoMock(IPopulator<Question, string> populator) : base(populator)
    {
    }
}