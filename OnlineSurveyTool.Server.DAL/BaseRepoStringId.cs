using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class BaseRepoStringId<T> : BaseRepo<T, string>, IRepoStringId<T> where T : EntityBaseStringId
{
    public BaseRepoStringId(OstDbContext dbContext) : base(dbContext)
    {
    }
}