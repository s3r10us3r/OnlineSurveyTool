using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces;

public interface IBaseRepo<T, in TId> : IDisposable where T : EntityBase<TId>
{
    Task<int> SaveChanges();
    Task<T?> GetOne(TId id);
    Task<int> Remove(TId id);
    Task<List<T>> GetAll();
    Task<int> Add(T entity);
    Task<int> AddRange(IList<T> entities);
    Task<int> Update(T entity);
    Task<int> Remove(T entity);
}