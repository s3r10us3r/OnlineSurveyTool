using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IRepoNumericId<T> : IBaseRepo<T, int> where T : EntityBaseIntegerId
    {
    }
}
