using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyResultRepo : BaseRepoNumericId<Models.SurveyResult>, ISurveyResultRepo
    {
        public SurveyResultRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
