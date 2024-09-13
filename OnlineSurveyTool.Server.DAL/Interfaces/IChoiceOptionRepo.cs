using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IChoiceOptionRepo : IRepoStringId<ChoiceOption>
    {
        public Task LoadAnswers(ChoiceOption choiceOption);
    }
}
