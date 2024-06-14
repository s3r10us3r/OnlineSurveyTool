using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IRepo<T> : IDisposable where T : EntityBase
    {
        int SaveChanges();
        T? GetOne(int id);
        List<T> GetAll();
        int Add(T entity);
        int AddRange(IList<T> entities);
        int Save(T entity);
        int Remove(int id);
        int Remove(T entity);
    }
}
