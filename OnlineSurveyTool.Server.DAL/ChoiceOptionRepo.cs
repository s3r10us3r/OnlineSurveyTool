using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class ChoiceOptionRepo : BaseRepoStringId<ChoiceOption>, IChoiceOptionRepo
    {
        public ChoiceOptionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
