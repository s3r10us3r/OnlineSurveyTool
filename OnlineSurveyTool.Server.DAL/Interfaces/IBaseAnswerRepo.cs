using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces;

public interface IBaseAnswerRepo<T>: IDisposable where T : Answer
{
     Task<int> SaveChanges();
     Task<T?> GetOne(int resultId, int number);
     Task<int> Remove(int resultId, int number);
     Task<List<T>> GetAll();
     Task<int> Add(T entity);
     Task<int> AddRange(IList<T> entities);
     Task<int> Update(T entity);
     Task<int> Remove(T entity);
}