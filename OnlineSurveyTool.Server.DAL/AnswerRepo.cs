using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerRepo : IAnswerRepo
{
    private readonly DbSet<Answer> _table;
    private readonly OstDbContext _db;
    
    public AnswerRepo(OstDbContext dbContext)
    {
        _db = dbContext;
        _table = dbContext.Answers;
    }
    
    public void Dispose()
    {
        _db?.Dispose();
    }

    public async Task<int> SaveChanges()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<Answer?> GetOne(int resultId, int number)
    {
        return await _table.FindAsync(resultId, number);
    }

    public async Task<int> Remove(int resultId, int number)
    {
        var answer = await GetOne(resultId, number);
        if (answer is null)
            return 0;
        return await Remove(answer);
    }

    public async Task<List<Answer>> GetAll()
    {
        return await _table.ToListAsync();
    }

    public async Task<int> Add(Answer entity)
    {
        await _table.AddAsync(entity);
        return await SaveChanges();
    }

    public async Task<int> AddRange(IList<Answer> entities)
    {
        await _table.AddRangeAsync(entities);
        return await SaveChanges();
    }

    public async Task<int> Update(Answer entity)
    {
        _table.Entry(entity).State = EntityState.Modified;
        return await SaveChanges();
    }

    public async Task<int> Remove(Answer entity)
    {
        _table.Remove(entity);
        return await SaveChanges();
    }

    public async Task<Answer> LoadAnswerOptions(Answer answer)
    {
        await _table.Entry(answer).Collection(a => a.AnswerOptions).LoadAsync();
        return answer;
    }
}
