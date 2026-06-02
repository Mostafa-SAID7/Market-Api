using Market.API.Data.Interfaces;

namespace Market.API.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work interface - coordinates multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Product repository
        /// </summary>
        IProductRepository Products { get; }

        /// <summary>
        /// Category repository
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// User repository
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Vendor repository
        /// </summary>
        IVendorRepository Vendors { get; }

        /// <summary>
        /// Order repository
        /// </summary>
        IOrderRepository Orders { get; }

        /// <summary>
        /// Cart repository
        /// </summary>
        ICartRepository Carts { get; }

        /// <summary>
        /// Review repository
        /// </summary>
        IReviewRepository Reviews { get; }

        /// <summary>
        /// Generic repository for other entities
        /// </summary>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Save all changes
        /// </summary>
        Task<int> SaveAsync();

        /// <summary>
        /// Begin transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        Task RollbackAsync();
    }
}
