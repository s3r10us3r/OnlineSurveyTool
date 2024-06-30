using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class QuestionRepo : BaseRepoStringId<Question>, IQuestionRepo
    {
        public QuestionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
