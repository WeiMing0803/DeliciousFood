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
    }
}