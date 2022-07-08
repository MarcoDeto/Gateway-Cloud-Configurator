using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ILogger<BaseRepository<T>> _logger;
        private readonly ConnContext _context;
        private DbSet<T> _entities;

        public BaseRepository(ConnContext connContext, ILogger<BaseRepository<T>> logger)
        {
            _context = connContext;
            _entities = _context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>()
               .ToListAsync();
        }

        public virtual async Task<T> Get(long id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual T Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _entities.Add(entity);
            Save();
            return entity;
        }

        public virtual bool Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _entities.Add(entity);
            return Save();
        }

        public virtual bool Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _entities.Update(entity);
            return Save();
        }

        public virtual bool Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _entities.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0;
        }
    }
}
