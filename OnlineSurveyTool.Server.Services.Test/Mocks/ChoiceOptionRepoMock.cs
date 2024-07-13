using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class ChoiceOptionRepoMock : BaseMock<ChoiceOption, string>, IChoiceOptionRepo
{
    public ChoiceOptionRepoMock(IPopulator<ChoiceOption, string> populator) : base(populator)
    {
    }
}