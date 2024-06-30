using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyResult : BaseRepoNumericId<Models.SurveyResult>, ISurveyResult
    {
        public SurveyResult(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
