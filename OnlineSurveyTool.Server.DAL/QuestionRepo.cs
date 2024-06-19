using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class QuestionRepo : BaseRepo<Question>, IQuestionRepo
    {
        public QuestionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
