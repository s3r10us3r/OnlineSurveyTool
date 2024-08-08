using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class SurveyRepoMock : BaseMock<Survey, string>, ISurveyRepo
{
    public SurveyRepoMock(IPopulator<Survey, string> populator) : base(populator)
    {
    }
}