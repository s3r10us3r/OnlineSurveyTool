using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public abstract class BaseMock<T>: IRepo<T> where T : EntityBase
{
    protected List<T> Entities => entities;
    
    private List<T> entities;
    private int changeTracker;

    protected BaseMock()
    {
        entities = new List<T>();
        changeTracker = 0;
        Populate();
    }

    protected abstract void Populate();
    
    public async Task<int> SaveChanges()
    {
        int result =  changeTracker;
        changeTracker = 0;
        return result;
    }

    public async Task<T?> GetOne(int id)
    {
        return entities.Find(e => e.Id == id);
    }

    public async Task<List<T>> GetAll()
    {
        return new List<T>(entities);
    }

    public async Task<int> Add(T entity)
    {
        if (entity.Id == 0)
        {
            lock (entities)
            {
                entity.Id = entities.Count;
                entities.Add(entity);
                return 1;
            }
        }
        if (await GetOne(entity.Id) is null)
        {
            lock (entities)
            {
                entities.Add(entity);
                return 1; 
            }
        }
        return 0;
    }

    public async Task<int> AddRange(IList<T> entities)
    {
        int result = 0;
        foreach (var entity in entities)
        {
            result += await Add(entity);
        }

        return result;
    }

    public async Task<int> Update(T entity)
    {
        var existingEntity = await GetOne(entity.Id);
        if (existingEntity is null)
        {
            return 0;
        }

        entities.Remove(existingEntity);
        entities.Add(entity);
        return 1;
    }

    public async Task<int> Remove(int id)
    {
        entities.RemoveAll(e => e.Id == id);
        return 1;
    }

    public async Task<int> Remove(T entity)
    {
        bool wasRemoved = entities.Remove(entity);
        return wasRemoved ? 1 : 0;
    }
    
    public void Dispose() 
    {
    }
}