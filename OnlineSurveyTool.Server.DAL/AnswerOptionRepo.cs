using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class AnswerOptionRepo : BaseRepoNumericId<AnswerOption>, IAnswerOptionRepo
    {
        public AnswerOptionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
