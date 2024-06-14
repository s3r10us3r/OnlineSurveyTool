using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }

        public int AddRange(IList<T> entities)
        {
            _table.AddRange(entities);
            return SaveChanges();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public T? GetOne(int id)
        {
            return _table.Find(id);
        }

        public int Remove(int id)
        {
            T? entity = _table.Find(id);
            if (entity is null)
            {
                return 0;
            }
            return Remove(entity);
        }

        public int Remove(T entity)
        {
            _table.Remove(entity);
            return SaveChanges();
        }

        public int Save(T entity)
        {
            _table.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }
}
