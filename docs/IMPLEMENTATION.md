# Implementation Guide

Complete implementation details for Market API v2.0 with MediatR, Validation, and Service layers.

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Entity Structure](#entity-structure)
- [MediatR Commands & Queries](#mediatr-commands--queries)
- [Validation Framework](#validation-framework)
- [Service Layer](#service-layer)
- [Repository Pattern](#repository-pattern)
- [Unit of Work](#unit-of-work)
- [Dependency Injection](#dependency-injection)

## Architecture Overview

Market API follows a **layered architecture** with **CQRS (Command Query Responsibility Segregation)** using MediatR:

```
┌─────────────────────────────┐
│   Presentation Layer        │ ← Controllers, HTTP
├─────────────────────────────┤
│   MediatR Layer             │ ← Commands, Queries, Handlers
├─────────────────────────────┤
│   Service Layer             │ ← Business Logic, Validation
├─────────────────────────────┤
│   Repository Layer          │ ← Data Access Abstraction
├─────────────────────────────┤
│   Data Layer                │ ← MongoDB, ORM
└─────────────────────────────┘
```

## Entity Structure

All entities inherit from `BaseEntity`:

```csharp
public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

### Core Entities

1. **Product** - Marketplace products with pricing, inventory, ratings
2. **Category** - Product categorization with slug and display order
3. **User** - Customer/vendor accounts with roles
4. **Vendor** - Store information with approval status and commission
5. **Order** - Customer orders with items, payment, and shipping
6. **Cart** - Shopping carts with line items
7. **Review** - Product reviews with ratings and verification

All entities use `[BsonIgnoreExtraElements]` for schema flexibility.

## MediatR Commands & Queries

### Command Pattern (Write Operations)

Commands represent **state-changing operations** (Create, Update, Delete).

**Structure**:
```csharp
// 1. Define the Command (Request)
public class CreateProductCommand : IRequest<ProductResponse>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 2. Implement the Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    
    public CreateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product: {Name}", request.Name);
        
        var product = new Product 
        { 
            Name = request.Name, 
            Price = request.Price 
        };
        
        await _unitOfWork.Products.CreateAsync(product);
        await _unitOfWork.SaveAsync();
        
        return new ProductResponse { Id = product.Id, Name = product.Name };
    }
}

// 3. Use in Controller
[HttpPost]
public async Task<IActionResult> Create(CreateProductCommand command)
{
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
}
```

**Available Commands by Entity**:
- `CreateProductCommand`, `UpdateProductCommand`, `DeleteProductCommand`
- `CreateCategoryCommand`, `UpdateCategoryCommand`, `DeleteCategoryCommand`
- `CreateUserCommand`, `UpdateUserCommand`, `DeleteUserCommand`
- `CreateVendorCommand`, `UpdateVendorCommand`, `DeleteVendorCommand`, `ApproveVendorCommand`
- `CreateOrderCommand`, `UpdateOrderCommand`, `DeleteOrderCommand`
- `AddToCartCommand`, `RemoveFromCartCommand`, `ClearCartCommand`
- `CreateReviewCommand`, `UpdateReviewCommand`, `DeleteReviewCommand`

### Query Pattern (Read Operations)

Queries represent **read-only operations** with no side effects.

**Structure**:
```csharp
// 1. Define the Query (Request)
public class GetAllProductsQuery : IRequest<List<ProductResponse>>
{
}

// 2. Implement the Handler
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;
    
    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllProductsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all products");
        
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(p => new ProductResponse 
        { 
            Id = p.Id, 
            Name = p.Name, 
            Price = p.Price 
        }).ToList();
    }
}

// 3. Use in Controller
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var result = await _mediator.Send(new GetAllProductsQuery());
    return Ok(result);
}
```

**Available Queries by Entity**:
- `GetAllProductsQuery`, `GetProductByIdQuery`, `GetProductsByCategoryQuery`
- `GetAllCategoriesQuery`, `GetCategoryByIdQuery`
- `GetAllUsersQuery`, `GetUserByIdQuery`
- `GetAllVendorsQuery`, `GetVendorByIdQuery`
- `GetAllOrdersQuery`, `GetOrderByIdQuery`
- `GetCartByUserIdQuery`
- `GetAllReviewsQuery`, `GetReviewsByProductIdQuery`

### Response DTOs

Each entity has a response DTO for API serialization:

```csharp
public class ProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

DTOs **exclude sensitive data** and **match API contracts**, not entity structures.

## Validation Framework

### Validation Architecture

```
Request Data
    ↓
Validation Middleware
    ↓
Service Validator
    ↓
Valid? → Proceed : Error Response
```

### Validator Interface

```csharp
public interface IValidator<T>
{
    Task<ValidationResult> ValidateAsync(T entity);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<ValidationError> Errors { get; set; } = new();
}

public class ValidationError
{
    public string Field { get; set; }
    public string Message { get; set; }
}
```

### Example Validator

```csharp
public class ProductValidator : IValidator<Product>
{
    public async Task<ValidationResult> ValidateAsync(Product entity)
    {
        var errors = new List<ValidationError>();
        
        // Required field
        if (string.IsNullOrWhiteSpace(entity.Name))
            errors.Add(new ValidationError("Name", "Product name is required"));
        
        // Length constraint
        if (entity.Name?.Length > 200)
            errors.Add(new ValidationError("Name", "Name cannot exceed 200 characters"));
        
        // Numeric range
        if (entity.Price <= 0)
            errors.Add(new ValidationError("Price", "Price must be positive"));
        
        // Format validation (URL)
        if (!string.IsNullOrEmpty(entity.ImageUrl) && !Uri.TryCreate(entity.ImageUrl, UriKind.Absolute, out _))
            errors.Add(new ValidationError("ImageUrl", "Invalid image URL format"));
        
        return new ValidationResult 
        { 
            IsValid = errors.Count == 0, 
            Errors = errors 
        };
    }
}
```

### Validation Middleware

```csharp
public class ValidationMiddleware
{
    private readonly RequestDelegate _next;
    
    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            var response = new { errors = ex.Errors };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

## Service Layer

### Service Pattern

Services encapsulate **business logic** and **cross-cutting concerns**.

```csharp
public interface IProductService
{
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<Product> GetByIdAsync(string id);
    Task<IEnumerable<Product>> GetAllAsync();
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<Product> _validator;
    private readonly ILogger<ProductService> _logger;
    
    public ProductService(
        IUnitOfWork unitOfWork,
        IValidator<Product> validator,
        ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Product> CreateAsync(Product product)
    {
        _logger.LogInformation("Creating product: {Name}", product.Name);
        
        // Validate
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Product validation failed: {Errors}", validationResult.Errors);
            throw new ValidationException(validationResult.Errors);
        }
        
        // Create
        await _unitOfWork.Products.CreateAsync(product);
        await _unitOfWork.SaveAsync();
        
        _logger.LogInformation("Product created: {ProductId}", product.Id);
        return product;
    }
}
```

### Service Responsibilities

1. **Validation** - Validate input using validators
2. **Business Logic** - Apply business rules
3. **Logging** - Log all operations
4. **Error Handling** - Handle and report errors
5. **Coordination** - Coordinate multiple repositories

## Repository Pattern

### Generic Repository

```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;
    protected readonly ILogger<Repository<T>> _logger;
    
    public Repository(IMongoCollection<T> collection, ILogger<Repository<T>> logger)
    {
        _collection = collection;
        _logger = logger;
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogInformation("Getting all {Entity}", typeof(T).Name);
        return await _collection.Find(_ => true).ToListAsync();
    }
    
    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    
    public virtual async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }
    
    public virtual async Task UpdateAsync(string id, T entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, entity);
    }
    
    public virtual async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
```

### Specific Repository

```csharp
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(
        IMongoCollection<Product> collection,
        ILogger<ProductRepository> logger)
        : base(collection, logger)
    {
    }
    
    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var filter = Builders<Product>.Filter.Gte(p => p.Price, minPrice) &
                    Builders<Product>.Filter.Lte(p => p.Price, maxPrice);
        
        return await _collection.Find(filter).ToListAsync();
    }
}
```

## Unit of Work

### Pattern Implementation

```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IUserRepository Users { get; }
    IVendorRepository Vendors { get; }
    IOrderRepository Orders { get; }
    ICartRepository Carts { get; }
    IReviewRepository Reviews { get; }
    
    Task<int> SaveAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    private readonly ILogger<UnitOfWork> _logger;
    
    private IProductRepository _productRepository;
    private ICategoryRepository _categoryRepository;
    private IUserRepository _userRepository;
    private IVendorRepository _vendorRepository;
    private IOrderRepository _orderRepository;
    private ICartRepository _cartRepository;
    private IReviewRepository _reviewRepository;
    
    public UnitOfWork(
        IMongoClient mongoClient,
        IOptions<MongoDbSettings> settings,
        ILogger<UnitOfWork> logger)
    {
        _mongoClient = mongoClient;
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _logger = logger;
    }
    
    public IProductRepository Products =>
        _productRepository ??= new ProductRepository(
            _database.GetCollection<Product>("Products"),
            new Logger<ProductRepository>(LoggerFactory.Create(builder => { })));
    
    // Similar for other repositories...
    
    public async Task<int> SaveAsync()
    {
        _logger.LogInformation("Saving changes");
        // MongoDB operations are auto-committed, so this is a no-op
        // But kept for compatibility with EF-style UoW pattern
        return await Task.FromResult(0);
    }
}
```

## Dependency Injection

### Configuration in Program.cs

```csharp
using Market.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddValidators();        // Register validators
builder.Services.AddMediatRServices();   // Register MediatR
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDataServices();      // Register repositories, UoW

var app = builder.Build();

// Initialize database
await app.InitializeDatabaseAsync();

// Configure pipeline
app.UseSwaggerDocumentation();
app.UseApplicationMiddleware();
app.MapApplicationRoutes();

app.Run();
```

### Configuration Classes

**ValidatorConfiguration.cs**:
```csharp
public static IServiceCollection AddValidators(this IServiceCollection services)
{
    services.AddScoped<IValidator<Product>, ProductValidator>();
    services.AddScoped<IValidator<Category>, CategoryValidator>();
    // ... all validators
    return services;
}
```

**MediatRConfiguration.cs**:
```csharp
public static IServiceCollection AddMediatRServices(this IServiceCollection services)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    return services;
}
```

**ServiceConfiguration.cs**:
```csharp
public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
{
    // MongoDB
    services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    services.AddSingleton<IMongoClient>(new MongoClient(configuration["MongoDbSettings:ConnectionString"]));
    
    // Services
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<IUserService, UserService>();
    // ... all services
    
    // Unit of Work & Repositories
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    return services;
}
```

## Best Practices

### 1. Use MediatR for All Requests

✅ **Good**: Use MediatR commands/queries
```csharp
var result = await _mediator.Send(new CreateProductCommand { ... });
```

❌ **Bad**: Call service directly
```csharp
var result = await _productService.CreateAsync(product);
```

### 2. Validate Before Processing

✅ **Good**: Validate in service or command handler
```csharp
var validationResult = await _validator.ValidateAsync(product);
if (!validationResult.IsValid)
    throw new ValidationException(validationResult.Errors);
```

### 3. Log Important Operations

✅ **Good**: Log with context
```csharp
_logger.LogInformation("Creating product: {Name} at price {Price}", product.Name, product.Price);
```

### 4. Use Dependency Injection

✅ **Good**: Constructor injection
```csharp
public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
{
    _unitOfWork = unitOfWork;
    _logger = logger;
}
```

❌ **Bad**: Service locator
```csharp
var service = serviceProvider.GetService<IProductService>();
```

### 5. Return Response DTOs

✅ **Good**: Map to DTO
```csharp
return new ProductResponse { Id = product.Id, Name = product.Name };
```

❌ **Bad**: Return entity
```csharp
return product;
```

## Future Enhancements

1. **MediatR Behaviors** - Add logging, validation, caching behaviors
2. **Pagination** - Add skip/take to queries
3. **Filtering** - Advanced filtering support in queries
4. **Sorting** - Sort query results
5. **Projection** - Select specific fields
6. **Specifications** - Specification pattern for complex queries
7. **AutoMapper** - Auto-map entities to DTOs
8. **Caching** - Redis caching layer
9. **Events** - Domain events with handlers
10. **Testing** - Unit and integration tests
