using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IRepo<T> : IDisposable where T : EntityBase
    {
        Task<int> SaveChanges();
        Task<T?> GetOne(int id);
        Task<List<T>> GetAll();
        Task<int> Add(T entity);
        Task<int> AddRange(IList<T> entities);
        Task<int> Update(T entity);
        Task<int> Remove(int id);
        Task<int> Remove(T entity);
    }
}
