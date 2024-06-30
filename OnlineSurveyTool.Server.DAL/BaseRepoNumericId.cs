using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class BaseRepoNumericId<T> : BaseRepo<T, int>, IRepoNumericId<T> where T : EntityBaseIntegerId, new()
    {
        public BaseRepoNumericId(OstDbContext dbContext) : base(dbContext)
        {
        }
    }
}
