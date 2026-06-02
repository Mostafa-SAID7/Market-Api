using Market.API.Data.Interfaces;
using Market.API.Data.Repositories;
using Market.API.Settings;
using Microsoft.Extensions.Options;

namespace Market.API.Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work implementation - coordinates all repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        private readonly IOptions<MongoDbSettings> _settings;
        private IProductRepository? _productRepository;
        private ICategoryRepository? _categoryRepository;
        private IUserRepository? _userRepository;
        private IVendorRepository? _vendorRepository;
        private IOrderRepository? _orderRepository;
        private ICartRepository? _cartRepository;
        private IReviewRepository? _reviewRepository;
        private Dictionary<Type, object>? _repositories;

        public UnitOfWork(MongoDbContext context, IOptions<MongoDbSettings> settings)
        {
            _context = context;
            _settings = settings;
            _repositories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Get product repository (lazy-loaded)
        /// </summary>
        public IProductRepository Products
        {
            get
            {
                _productRepository ??= new ProductRepository(_settings);
                return _productRepository;
            }
        }

        /// <summary>
        /// Get category repository (lazy-loaded)
        /// </summary>
        public ICategoryRepository Categories
        {
            get
            {
                _categoryRepository ??= new CategoryRepository(_settings);
                return _categoryRepository;
            }
        }

        /// <summary>
        /// Get user repository (lazy-loaded)
        /// </summary>
        public IUserRepository Users
        {
            get
            {
                _userRepository ??= new UserRepository(_settings);
                return _userRepository;
            }
        }

        /// <summary>
        /// Get vendor repository (lazy-loaded)
        /// </summary>
        public IVendorRepository Vendors
        {
            get
            {
                _vendorRepository ??= new VendorRepository(_settings);
                return _vendorRepository;
            }
        }

        /// <summary>
        /// Get order repository (lazy-loaded)
        /// </summary>
        public IOrderRepository Orders
        {
            get
            {
                _orderRepository ??= new OrderRepository(_settings);
                return _orderRepository;
            }
        }

        /// <summary>
        /// Get cart repository (lazy-loaded)
        /// </summary>
        public ICartRepository Carts
        {
            get
            {
                _cartRepository ??= new CartRepository(_settings);
                return _cartRepository;
            }
        }

        /// <summary>
        /// Get review repository (lazy-loaded)
        /// </summary>
        public IReviewRepository Reviews
        {
            get
            {
                _reviewRepository ??= new ReviewRepository(_settings);
                return _reviewRepository;
            }
        }

        /// <summary>
        /// Get generic repository (lazy-loaded with caching)
        /// </summary>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories!.ContainsKey(type))
                return (IRepository<T>)_repositories[type];

            var repositoryType = typeof(Repository<>).MakeGenericType(type);
            var repository = (IRepository<T>)Activator.CreateInstance(repositoryType, _settings)!;
            _repositories.Add(type, repository);
            return repository;
        }

        /// <summary>
        /// Save changes (MongoDB doesn't have transactions by default,
        /// but this is here for API compatibility and future enhancements)
        /// </summary>
        public async Task<int> SaveAsync()
        {
            // MongoDB doesn't require explicit saves
            // This is here for compatibility with EF Core patterns
            await Task.CompletedTask;
            return 0;
        }

        /// <summary>
        /// Begin transaction (for future MongoDB transaction support)
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            // MongoDB transactions require replica set (not available in MongoDB Atlas free tier)
            // Placeholder for future implementation
            await Task.CompletedTask;
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public async Task CommitAsync()
        {
            // Placeholder for future implementation
            await Task.CompletedTask;
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public async Task RollbackAsync()
        {
            // Placeholder for future implementation
            await Task.CompletedTask;
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _repositories?.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
