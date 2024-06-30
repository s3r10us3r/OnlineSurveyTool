using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class AnswerRepo : BaseRepoNumericId<Answer>, IAnswerRepo
    {
        public AnswerRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
