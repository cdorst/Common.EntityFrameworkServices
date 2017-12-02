using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public class Repository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly TDbContext _context;
        private readonly DbSet<TEntity> _set;
        protected readonly ILogger<Repository<TDbContext, TEntity>> _logger;

        public Repository(TDbContext context, ILogger<Repository<TDbContext, TEntity>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _set = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null) return null;
            _logger.LogInformation("Adding entity to db context");
            _set.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            if ((keyValues?.Length ?? 0) == 0) return null;
            _logger.LogInformation("Finding entity");
            return await _set.FindAsync(keyValues);
        }

        public virtual async Task RemoveAsync(params object[] keyValues)
        {
            if ((keyValues?.Length ?? 0) == 0) return;
            _logger.LogInformation("Removing entity");
            var entity = await FindAsync(keyValues);
            if (entity == null) return;
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) return null;
            _logger.LogInformation("Updating entity");
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
