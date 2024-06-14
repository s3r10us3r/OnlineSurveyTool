using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyResultRepo : BaseRepo<SurveyResult>, ISurveyResultRepo
    {
        public SurveyResultRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }
    }
}
