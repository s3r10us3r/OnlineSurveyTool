using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class ChoiceOptionRepo : BaseRepoStringId<ChoiceOption>, IChoiceOptionRepo
    {
        public ChoiceOptionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }

        public async Task LoadAnswers(ChoiceOption choiceOption)
        {
            await Context.Entry(choiceOption).Collection(co => co.Answers).LoadAsync();
        }
    }
}
