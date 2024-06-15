using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class BaseRepo<T> : IRepo<T> where T : EntityBase, new()
    {
        private readonly DbSet<T> _table;
        private readonly OSTDbContext _db;

        protected OSTDbContext Context => _db;
        protected DbSet<T> Table => _table;

        public BaseRepo(OSTDbContext dbContext)
        {
            _db = dbContext;
            _table = _db.Set<T>();
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

        public void Dispose()
        {
            _db?.Dispose();
        }

        public async Task<List<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T?> GetOne(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<int> Remove(int id)
        {
            T? entity = await _table.FindAsync(id);
            if (entity is null)
            {
                return 0;
            }
            return await Remove(entity);
        }

        public async Task<int> Remove(T entity)
        {
            _table.Remove(entity);
            return await SaveChanges();
        }

        public Task<int> Update(T entity)
        {
            _table.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
