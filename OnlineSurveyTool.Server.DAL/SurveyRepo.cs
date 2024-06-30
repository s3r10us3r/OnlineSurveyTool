using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyRepo : BaseRepoStringId<Survey>, ISurveyRepo
    {
        public SurveyRepo(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
