using Market.Domain.Repositories;
using Market.Infrastructure.Data.Repositories;

namespace Market.Infrastructure.Data.Persistence
{
    /// <summary>
    /// Unit of Work implementation - thin wrapper around EF Core DbContext
    /// Provides repository-based access to data with lazy loading
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketDbContext _context;
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        private IUserRepository? _userRepository;
        private IVendorRepository? _vendorRepository;
        private IOrderRepository? _orderRepository;
        private ICartRepository? _cartRepository;
        private IReviewRepository? _reviewRepository;

        public UnitOfWork(MarketDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Product repository (lazy-loaded)
        /// </summary>
        public IProductRepository Products
        {
            get { return _productRepository ??= new ProductRepository(_context); }
        }

        /// <summary>
        /// Category repository (lazy-loaded)
        /// </summary>
        public ICategoryRepository Categories
        {
            get { return _categoryRepository ??= new CategoryRepository(_context); }
        }

        /// <summary>
        /// User repository (lazy-loaded)
        /// </summary>
        public IUserRepository Users
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        /// <summary>
        /// Vendor repository (lazy-loaded)
        /// </summary>
        public IVendorRepository Vendors
        {
            get { return _vendorRepository ??= new VendorRepository(_context); }
        }

        /// <summary>
        /// Order repository (lazy-loaded)
        /// </summary>
        public IOrderRepository Orders
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }

        /// <summary>
        /// Cart repository (lazy-loaded)
        /// </summary>
        public ICartRepository Carts
        {
            get { return _cartRepository ??= new CartRepository(_context); }
        }

        /// <summary>
        /// Review repository (lazy-loaded)
        /// </summary>
        public IReviewRepository Reviews
        {
            get { return _reviewRepository ??= new ReviewRepository(_context); }
        }

        /// <summary>
        /// Save all changes to the database asynchronously
        /// </summary>
        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

