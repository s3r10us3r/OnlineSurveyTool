using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class ChoiceOptionRepoMock : BaseMock<ChoiceOption, string>, IChoiceOptionRepo
{
    public ChoiceOptionRepoMock(IPopulator<ChoiceOption, string> populator) : base(populator)
    {
    }
}