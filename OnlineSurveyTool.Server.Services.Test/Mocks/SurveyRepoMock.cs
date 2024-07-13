using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class SurveyRepoMock : BaseMock<Survey, string>, ISurveyRepo
{
    public SurveyRepoMock(IPopulator<Survey, string> populator) : base(populator)
    {
    }
}