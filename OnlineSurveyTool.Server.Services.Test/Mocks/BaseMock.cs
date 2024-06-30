using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public abstract class BaseMock<T, TId>: IBaseRepo<T, TId> where T : EntityBase<TId>, new()
{
    private readonly List<T> _entities;
    protected readonly object _entitiesLock;
    protected List<T> Entities => _entities;
    public BaseMock(IPopulator<T, TId> populator)
    {
        _entities = populator.Populate();
    }
    
    public async Task<int> SaveChanges()
    {
        return 0;
    }

    public async Task<T?> GetOne(TId id)
    {
        return _entities.Find(e => Equals(e.Id, id));
    }

    public async Task<int> Remove(TId id)
    {
        lock (_entitiesLock)
        return _entities.RemoveAll(e => Equals(e.Id, id));
    }

    public async Task<List<T>> GetAll()
    {
        return _entities.ToList();
    }

    public async Task<int> Add(T entity)
    {
        Console.WriteLine(entity.Id);
        var ent = await GetOne(entity.Id);
        if (ent is not null)
        {
            return 0;
        }
        _entities.Add(ent);
        return 1;
    }

    public async Task<int> AddRange(IList<T> entities)
    {
        int sum = 0;
        foreach (var entity in entities)
        {
            sum += await Add(entity);
        }

        return sum;
    }

    public async Task<int> Update(T entity)
    {
        var ent = await GetOne(entity.Id);
        if (ent is null)
        {
            return 0;
        }

        _entities.Remove(ent);
        _entities.Add(entity);
        return 1;
    }

    public async Task<int> Remove(T entity)
    {
        return _entities.Remove(entity) ? 1 : 0;
    }
    
    
    public void Dispose()
    {
    }
}