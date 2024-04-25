using Microsoft.EntityFrameworkCore;

namespace AI.DeliciousFood.Core.Data
{
    public class GenericRepository<TContext>(TContext _context) where TContext : DbContext
    {
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            List<TEntity> list = await _context.Set<TEntity>().ToListAsync(cancellationToken);
            return list;
        }
    }
}