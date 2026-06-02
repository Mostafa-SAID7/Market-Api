using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Services.Interfaces;
using Market.API.Validators;
using Market.API.Middleware;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling product business logic
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IValidator<Product> _validator;

        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger, IValidator<Product> validator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching all products");
            return await _unitOfWork.Products.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            _logger.LogInformation("Fetching products in price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
            
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                _logger.LogWarning("Invalid price range provided: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
                return Enumerable.Empty<Product>();
            }

            return await _unitOfWork.Products.GetByPriceRange(minPrice, maxPrice);
        }

        /// <inheritdoc/>
        public async Task<Product> CreateProductAsync(Product product)
        {
            _logger.LogInformation("Creating new product: {ProductName}", product.Name);
            
            // Validate product
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Product validation failed with {ErrorCount} errors", validationResult.Errors.Count);
                throw new Middleware.ValidationException(validationResult);
            }

            await _unitOfWork.Products.CreateAsync(product);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
            return product;
        }

        /// <inheritdoc/>
        public async Task<Product> UpdateProductAsync(string id, Product product)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);
            
            // Validate product
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Product validation failed with {ErrorCount} errors", validationResult.Errors.Count);
                throw new Middleware.ValidationException(validationResult);
            }

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product not found for update: {ProductId}", id);
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            product.Id = id;
            await _unitOfWork.Products.UpdateAsync(id, product);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Product updated successfully: {ProductId}", id);
            return product;
        }

        /// <inheritdoc/>
        public async Task DeleteProductAsync(string id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);
            
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found for deletion: {ProductId}", id);
                throw new KeyNotFoundException($"Product with ID {id} not found");
            }

            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Product deleted successfully: {ProductId}", id);
        }
    }
}
