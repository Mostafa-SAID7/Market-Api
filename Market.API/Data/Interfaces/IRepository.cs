namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities (including soft-deleted if needed)
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(string id);

        /// <summary>
        /// Create new entity
        /// </summary>
        Task CreateAsync(T entity);

        /// <summary>
        /// Update existing entity
        /// </summary>
        Task UpdateAsync(string id, T entity);

        /// <summary>
        /// Delete entity (soft delete supported via BaseEntity)
        /// </summary>
        Task DeleteAsync(string id);
    }
}
