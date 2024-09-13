using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class BaseRepoAnswer<T>: IBaseAnswerRepo<T>, IAsyncDisposable where T : Answer
{
    private readonly DbSet<T> _table;
    private readonly OstDbContext _dbContext;
    
    protected BaseRepoAnswer(OstDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<T>();
    }


    public virtual async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<T?> GetOne(int resultId, int number)
    {
        return await _table.FindAsync(resultId, number);
    }

    public virtual async Task<int> Remove(int resultId, int number)
    {
        var ent = await GetOne(resultId, number);
        if (ent is null)
            return 0;
        return await Remove(ent);
    }

    public async Task<List<T>> GetAll()
    {
        return await _table.ToListAsync();
    }

    public async Task<int> Add(T entity)
    {
        await _table.AddAsync(entity);
        return await SaveChanges();
    }

    public async Task<int> AddRange(IList<T> entities)
    {
        await _table.AddRangeAsync(entities);
        return await SaveChanges();
    }

    public async Task<int> Update(T entity)
    {
        _table.Entry(entity).State = EntityState.Modified;
        return await SaveChanges();
    }

    public async Task<int> Remove(T entity)
    {
        _table.Remove(entity);
        return await SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}