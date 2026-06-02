# Architecture Guide

This document describes the architecture and design patterns used in the Market API project.

## Table of Contents

- [Overview](#overview)
- [Architecture Pattern](#architecture-pattern)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Data Flow](#data-flow)
- [Key Components](#key-components)
- [Best Practices](#best-practices)

## Overview

Market API is built using **ASP.NET Core 9** with a focus on:
- Clean Architecture principles
- Repository Pattern for data access
- Dependency Injection
- Separation of Concerns
- SOLID principles

## Architecture Pattern

### Repository Pattern

The Repository Pattern abstracts the data access layer, providing a collection-like interface for accessing domain objects.

**Benefits**:
- Decouples business logic from data access
- Easier to test (can mock repositories)
- Centralized data access logic
- Easier to switch data sources

```
┌─────────────────────────────────────────────┐
│           Presentation Layer                │
│         (Controllers/API)                   │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│         Business Logic Layer                │
│         (Services - Future)                 │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│         Repository Layer                    │
│    (IRepository, IProductRepository)        │
└─────────────────┬───────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────┐
│         Data Access Layer                   │
│         (MongoDB Driver)                    │
└─────────────────────────────────────────────┘
```

## Project Structure

```
Market.API/
├── Controllers/              # HTTP Request Handlers
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── UsersController.cs
│   ├── VendorsController.cs
│   ├── OrdersController.cs
│   ├── CartsController.cs
│   └── ReviewsController.cs
│
├── Features/                 # MediatR Commands & Queries
│   ├── Products/
│   │   ├── Commands/        # Create, Update, Delete
│   │   ├── Queries/         # GetAll, GetById, GetByCategory
│   │   └── ProductResponse.cs
│   ├── Categories/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   └── CategoryResponse.cs
│   ├── Users, Vendors, Orders, Carts, Reviews/  # Same structure
│   └── (Similar for all 7 entities)
│
├── Models/
│   ├── Entities/             # Domain Models
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── User.cs
│   │   ├── Vendor.cs
│   │   ├── Order.cs
│   │   ├── Cart.cs
│   │   └── Review.cs
│   └── Enums/                # OrderStatus, PaymentStatus, UserRole
│
├── Data/
│   ├── Repositories/         # Data Access Implementation
│   │   ├── Repository.cs     # Generic implementation
│   │   └── (Specific repos for each entity)
│   ├── Interfaces/           # Repository Contracts
│   │   ├── IRepository.cs    # Generic interface
│   │   └── (Specific interfaces)
│   ├── UnitOfWork/           # Coordinated repository access
│   └── MongoDbContext.cs    # MongoDB connection & indexes
│
├── Services/                 # Business Logic Layer
│   ├── ProductService.cs
│   ├── Interfaces/
│   │   └── IProductService.cs
│   └── (Similar for all entities)
│
├── Validators/               # Validation Framework
│   ├── ProductValidator.cs
│   ├── ValidationResult.cs
│   ├── ValidationError.cs
│   └── (Similar for all entities)
│
├── Configurations/           # DI Setup & Middleware
│   ├── ServiceConfiguration.cs
│   ├── DataConfiguration.cs
│   ├── ValidatorConfiguration.cs
│   ├── MediatRConfiguration.cs
│   ├── SwaggerConfiguration.cs
│   └── MiddlewareConfiguration.cs
│
├── Middleware/               # Request Processing
│   └── ValidationMiddleware.cs
│
├── Settings/                 # Configuration Models
│   └── MongoDbSettings.cs
│
├── Program.cs               # Application Entry Point
└── appsettings.json         # Configuration
```

## Design Patterns

### 1. Repository Pattern

**Generic Repository** (`IRepository<T>`):
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}
```

**Specific Repository** (`IProductRepository`):
```csharp
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByPriceRange(decimal minPrice, decimal maxPrice);
}
```

### 2. Dependency Injection

Services are registered in `Program.cs`:

```csharp
// MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Product Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

### 3. Options Pattern

Configuration is bound to strongly-typed classes:

```csharp
public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
```

## Data Flow

### Request Flow with MediatR

```
1. HTTP Request
   ↓
2. Controller (e.g., ProductsController)
   ↓
3. MediatR.Send(Command or Query)
   ↓
4. Command/Query Handler
   ├─ Validates request
   ├─ Calls Service (if needed)
   └─ Calls Repository
      ↓
5. Service Layer (optional business logic)
   ├─ Validates entity
   ├─ Applies business rules
   └─ Calls Repository
      ↓
6. Repository Layer
   ├─ Applies MongoDB operations
   └─ Calls MongoDB Driver
      ↓
7. MongoDB Database
   ↓
8. Handler returns Response DTO
   ↓
9. HTTP Response (JSON)
```

### Example: Create Product Request

```csharp
// 1. HTTP POST request
POST /api/products
{
  "name": "Laptop",
  "price": 999.99
}

// 2. Controller receives request
[HttpPost]
public async Task<IActionResult> Create(CreateProductCommand command)
{
    // 3. Send to MediatR
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
}

// 4. Handler processes command
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 5. Create entity
        var product = new Product 
        { 
            Name = request.Name, 
            Price = request.Price 
        };
        
        // 6. Call repository
        await _repository.CreateAsync(product);
        
        // 7. Return response DTO
        return new ProductResponse 
        { 
            Id = product.Id, 
            Name = product.Name, 
            Price = product.Price 
        };
    }
}

// 8. Response sent back
201 Created
{
  "id": "507f1f77bcf86cd799439011",
  "name": "Laptop",
  "price": 999.99
}
```

## Key Components

### 1. Controllers

**Responsibility**: Handle HTTP requests and route to MediatR

**Example**:
```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return Ok(result);
    }
}
```

### 2. MediatR Commands & Queries

**Responsibility**: Encapsulate request/response logic with handlers

**Commands** (Create, Update, Delete):
```csharp
public class CreateProductCommand : IRequest<ProductResponse>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    // Handle business logic
}
```

**Queries** (Read):
```csharp
public class GetAllProductsQuery : IRequest<List<ProductResponse>> { }

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
    // Handle query logic
}
```

### 3. Response DTOs

**Responsibility**: Define API response contracts

**Example**:
```csharp
public class ProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### 4. Services

**Responsibility**: Business logic layer with validation and logging

**Example**:
```csharp
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IValidator<Product> _validator;
    
    public async Task<Product> CreateAsync(Product product)
    {
        // Validate
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        // Create
        await _repository.CreateAsync(product);
        return product;
    }
}
```

### 5. Validators

**Responsibility**: Validate entities before processing

**Example**:
```csharp
public class ProductValidator : IValidator<Product>
{
    public async Task<ValidationResult> ValidateAsync(Product entity)
    {
        var errors = new List<ValidationError>();
        
        if (string.IsNullOrWhiteSpace(entity.Name))
            errors.Add(new ValidationError("Name", "Product name is required"));
            
        if (entity.Price <= 0)
            errors.Add(new ValidationError("Price", "Price must be positive"));
        
        return new ValidationResult { Errors = errors, IsValid = errors.Count == 0 };
    }
}
```

### 6. Repositories

**Responsibility**: Abstract data access logic

**Generic Repository**:
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}
```

**Specific Repository**:
```csharp
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal min, decimal max);
}
```

### 7. Entities

**Responsibility**: Define domain models

**Example**:
```csharp
[BsonIgnoreExtraElements]
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}
```

### 8. Unit of Work

**Responsibility**: Coordinate multiple repositories

**Example**:
```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    IVendorRepository Vendors { get; }
    // ... other repositories
    Task<int> SaveAsync();
}
```

## Best Practices

### 1. Async/Await

All data access operations use async/await:

```csharp
public async Task<IEnumerable<Product>> GetAllAsync()
{
    return await _collection.Find(_ => true).ToListAsync();
}
```

### 2. Dependency Injection

Use constructor injection for dependencies:

```csharp
public ProductsController(IProductRepository repository)
{
    _repository = repository;
}
```

### 3. Interface Segregation

Separate interfaces for different concerns:

```csharp
IRepository<T>           // Generic operations
IProductRepository       // Product-specific operations
```

### 4. Single Responsibility

Each class has one reason to change:

- **Controllers**: Handle HTTP
- **Repositories**: Handle data access
- **Entities**: Define data structure

### 5. Configuration Management

Use strongly-typed configuration:

```csharp
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));
```

## Future Enhancements

### 1. Service Layer

Add a service layer between controllers and repositories:

```
Controller → Service → Repository → Database
```

### 2. Unit of Work Pattern

Implement Unit of Work for transaction management:

```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> CompleteAsync();
}
```

### 3. CQRS Pattern

Separate read and write operations:

```csharp
IProductQueryRepository  // Read operations
IProductCommandRepository // Write operations
```

### 4. Domain Events

Implement domain events for loose coupling:

```csharp
public class ProductCreatedEvent : IDomainEvent
{
    public Product Product { get; set; }
}
```

### 5. Validation Layer

Add FluentValidation for request validation:

```csharp
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

## Testing Strategy

### Unit Tests

Test repositories with mocked MongoDB:

```csharp
[Fact]
public async Task GetAllAsync_ReturnsAllProducts()
{
    // Arrange
    var mockCollection = new Mock<IMongoCollection<Product>>();
    var repository = new Repository<Product>(mockCollection.Object);
    
    // Act
    var result = await repository.GetAllAsync();
    
    // Assert
    Assert.NotNull(result);
}
```

### Integration Tests

Test with real MongoDB (TestContainers):

```csharp
[Fact]
public async Task CreateProduct_SavesToDatabase()
{
    // Arrange
    using var container = new MongoDbContainer();
    await container.StartAsync();
    
    // Act & Assert
    // Test with real MongoDB
}
```

## Performance Considerations

### 1. Indexing

Create indexes for frequently queried fields:

```csharp
await collection.Indexes.CreateOneAsync(
    new CreateIndexModel<Product>(
        Builders<Product>.IndexKeys.Ascending(x => x.Price)
    )
);
```

### 2. Projection

Use projection to retrieve only needed fields:

```csharp
var products = await _collection
    .Find(filter)
    .Project(x => new { x.Id, x.Name })
    .ToListAsync();
```

### 3. Pagination

Implement pagination for large datasets:

```csharp
public async Task<IEnumerable<Product>> GetPagedAsync(int page, int pageSize)
{
    return await _collection
        .Find(_ => true)
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();
}
```

## Security Considerations

### 1. Input Validation

Validate all user inputs:

```csharp
[Required]
[StringLength(100)]
public string Name { get; set; }

[Range(0.01, double.MaxValue)]
public decimal Price { get; set; }
```

### 2. Connection String Security

Store connection strings securely:
- Use User Secrets in development
- Use Azure Key Vault in production
- Never commit connection strings to source control

### 3. API Security

Consider adding:
- Authentication (JWT)
- Authorization (Role-based)
- Rate limiting
- CORS configuration

## Monitoring and Logging

### Recommended Additions

1. **Structured Logging**: Use Serilog
2. **Health Checks**: Monitor MongoDB connection
3. **Metrics**: Track API performance
4. **Distributed Tracing**: Use OpenTelemetry

## Conclusion

This architecture provides:
- ✅ Clean separation of concerns
- ✅ Testable code
- ✅ Maintainable structure
- ✅ Scalable design
- ✅ SOLID principles

The design allows for easy extension and modification as the application grows.
