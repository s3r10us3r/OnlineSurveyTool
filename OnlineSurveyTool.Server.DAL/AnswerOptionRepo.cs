using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class AnswerOptionRepo : BaseRepo<AnswerOption>, IAnswerOptionRepo
    {
        public AnswerOptionRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
