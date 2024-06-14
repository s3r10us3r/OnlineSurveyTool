using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class AnswerRepo : BaseRepo<Answer>, IAnswerRepo
    {
        public AnswerRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }
    }
}
