using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class ChoiceOptionRepo : BaseRepo<ChoiceOption>, IChoiceOptionRepo
    {
        public ChoiceOptionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
