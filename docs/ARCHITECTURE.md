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
│   └── ProductsController.cs
│
├── Entities/                 # Domain Models
│   └── Product.cs
│
├── Repository/               # Data Access Abstraction
│   ├── IRepository.cs       # Generic Repository Interface
│   ├── Repository.cs        # Generic Repository Implementation
│   ├── IProductRepository.cs # Product-Specific Interface
│   └── ProductRepository.cs  # Product-Specific Implementation
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

### Request Flow

```
1. HTTP Request
   ↓
2. Controller (ProductsController)
   ↓
3. Repository (IProductRepository)
   ↓
4. MongoDB Driver
   ↓
5. MongoDB Database
   ↓
6. Response (JSON)
```

### Example: Create Product

```csharp
// 1. Controller receives request
[HttpPost]
public async Task<IActionResult> Create(Product product)
{
    // 2. Calls repository
    await _repository.CreateAsync(product);
    
    // 3. Returns response
    return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
}

// 4. Repository implementation
public async Task CreateAsync(T entity)
{
    // 5. MongoDB operation
    await _collection.InsertOneAsync(entity);
}
```

## Key Components

### 1. Controllers

**Responsibility**: Handle HTTP requests and responses

**Example**:
```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    
    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }
}
```

### 2. Entities

**Responsibility**: Define domain models

**Example**:
```csharp
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 3. Repositories

**Responsibility**: Abstract data access logic

**Generic Repository**:
- Provides common CRUD operations
- Reusable across different entities

**Specific Repository**:
- Extends generic repository
- Adds entity-specific operations

### 4. Settings

**Responsibility**: Configuration management

**Example**:
```csharp
public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
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
