using Market.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic repository implementation for EF Core
    /// </summary>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MarketDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(MarketDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Get all non-deleted entities
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get entity by ID
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Create new entity
        /// </summary>
        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Soft delete entity
        /// </summary>
        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity != null)
            {
                entity.Delete();
                _dbSet.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Check if entity exists
        /// </summary>
        public virtual async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }
    }
}



