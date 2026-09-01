namespace Market.Domain.Repositories
{
    /// <summary>
    /// Unit of Work interface - provides coordinated repository access
    /// This is a thin wrapper around the DbContext for backward compatibility.
    /// In EF Core, the DbContext itself IS the Unit of Work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IVendorRepository Vendors { get; }
        IOrderRepository Orders { get; }
        ICartRepository Carts { get; }
        IReviewRepository Reviews { get; }

        /// <summary>
        /// Save all changes to the database asynchronously
        /// </summary>
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}

