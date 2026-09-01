# Architecture Guide

This document describes the strict 4-layer Clean Architecture used in the Market API project.

## Table of Contents

- [Overview](#overview)
- [4-Layer Architecture](#4-layer-architecture)
- [Project Structure](#project-structure)
- [Layer Responsibilities](#layer-responsibilities)
- [Dependency Flow](#dependency-flow)
- [Design Patterns](#design-patterns)
- [SOLID Principles](#solid-principles)
- [Data Flow](#data-flow)
- [Technology Stack](#technology-stack)

## Overview

Market API is built using **ASP.NET Core 9** with a focus on:
- **Strict Clean Architecture** (4-layer separation)
- **Pure CQRS** (Command/Query Responsibility Segregation)
- **Dependency Inversion** (Domain interfaces, Infrastructure implementations)
- **Repository Pattern** for data access
- **Entity Framework Core 9.0** with SQL Server
- **SOLID Principles** throughout
- **Zero Duplication** and tight coupling prevention

## 4-Layer Architecture

```
┌─────────────────────────────────────────┐
│  API Layer (Presentation)               │  ← Depends on: Application + Infrastructure
│  • Controllers                          │
│  • Middleware                           │
│  • Program.cs / Configuration           │
│  • Zero Business Logic                  │
└─────────────────┬───────────────────────┘
                  │
        ┌─────────▼─────────┐
        │  (MediatR CQRS)   │
        └─────────┬─────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Application Layer                      │  ← Depends on: Domain only
│  • Commands & Queries                   │
│  • CQRS Handlers (MediatR)              │
│  • DTOs / Response Models               │
│  • Validators (FluentValidation)        │
│  • No EF Core, No Dependencies          │
└─────────────────┬───────────────────────┘
                  │
                  │ (abstraction)
                  │
┌─────────────────▼───────────────────────┐
│  Infrastructure Layer                   │  ← Depends on: Application + Domain
│  • DbContext (EF Core)                  │
│  • Repository Implementations           │
│  • Unit of Work                         │
│  • Data Seeds                           │
│  • Migrations                           │
│  • External Services                    │
└─────────────────┬───────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────┐
│  Domain Layer (Core)                    │  ← Depends on: NOTHING
│  • Entities                             │
│  • Enums                                │
│  • Value Objects                        │
│  • Repository Interfaces (IRepo*)       │
│  • IUnitOfWork                          │
│  • Zero External Dependencies           │
└─────────────────────────────────────────┘
```

## Project Structure

```
Market-Api/
│
├── src/
│   │
│   ├── Market.Domain/                  (Innermost - Zero Dependencies)
│   │   ├── Entities/
│   │   │   ├── Product.cs
│   │   │   ├── Category.cs
│   │   │   ├── User.cs
│   │   │   ├── Vendor.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   ├── Cart.cs
│   │   │   ├── CartItem.cs
│   │   │   └── Review.cs
│   │   │
│   │   ├── Enums/
│   │   │   ├── OrderStatus.cs
│   │   │   ├── PaymentStatus.cs
│   │   │   ├── UserRole.cs
│   │   │   ├── ProductStatus.cs
│   │   │   └── VendorApprovalStatus.cs
│   │   │
│   │   ├── ValueObjects/
│   │   │   └── Slug.cs
│   │   │
│   │   ├── Common/
│   │   │   └── BaseEntity.cs
│   │   │
│   │   ├── Repositories/  (Abstractions Only)
│   │   │   ├── IRepository.cs
│   │   │   ├── IProductRepository.cs
│   │   │   ├── ICategoryRepository.cs
│   │   │   ├── IUserRepository.cs
│   │   │   ├── IVendorRepository.cs
│   │   │   ├── IOrderRepository.cs
│   │   │   ├── ICartRepository.cs
│   │   │   ├── IReviewRepository.cs
│   │   │   └── IUnitOfWork.cs
│   │   │
│   │   └── Market.Domain.csproj
│   │
│   ├── Market.Application/             (Depends on Domain)
│   │   ├── DependencyInjection.cs       (Registers MediatR + Validators)
│   │   │
│   │   ├── Features/                    (Organized by Feature)
│   │   │   ├── Products/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   │   ├── UpdateProductCommand.cs
│   │   │   │   │   └── DeleteProductCommand.cs
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetAllProductsQuery.cs
│   │   │   │   │   ├── GetProductByIdQuery.cs
│   │   │   │   │   └── GetProductsByCategoryQuery.cs
│   │   │   │   ├── Handlers/
│   │   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   │   ├── GetAllProductsQueryHandler.cs
│   │   │   │   │   └── ...
│   │   │   │   └── ProductResponse.cs   (DTO)
│   │   │   │
│   │   │   ├── Orders/
│   │   │   ├── Users/
│   │   │   ├── Vendors/
│   │   │   ├── Categories/
│   │   │   ├── Carts/
│   │   │   └── Reviews/
│   │   │
│   │   ├── Validators/                  (FluentValidation Rules)
│   │   │   ├── CreateProductValidator.cs
│   │   │   ├── UpdateProductValidator.cs
│   │   │   └── ...
│   │   │
│   │   └── Market.Application.csproj
│   │
│   ├── Market.Infrastructure/          (Depends on Application + Domain)
│   │   ├── DependencyInjection.cs       (Registers DbContext + Repos + UnitOfWork)
│   │   │
│   │   ├── Data/
│   │   │   ├── MarketDbContext.cs       (EF Core DbContext)
│   │   │   │
│   │   │   ├── Configurations/          (EF Core Fluent API)
│   │   │   │   ├── ProductConfiguration.cs
│   │   │   │   ├── CategoryConfiguration.cs
│   │   │   │   └── ...
│   │   │   │
│   │   │   ├── Persistence/
│   │   │   │   └── UnitOfWork.cs        (IUnitOfWork Implementation)
│   │   │   │
│   │   │   ├── Repositories/            (IRepository Implementations)
│   │   │   │   ├── Repository.cs        (Generic Base)
│   │   │   │   ├── ProductRepository.cs
│   │   │   │   ├── OrderRepository.cs
│   │   │   │   └── ...
│   │   │   │
│   │   │   ├── Seeds/
│   │   │   │   └── DataSeeder.cs        (Initial Data)
│   │   │   │
│   │   │   └── Migrations/              (EF Core Migrations)
│   │   │       ├── 001_InitialCreate.cs
│   │   │       └── ...
│   │   │
│   │   └── Market.Infrastructure.csproj
│   │
│   ├── Market.API/                     (Depends on Application + Infrastructure)
│   │   ├── Controllers/
│   │   │   ├── ProductsController.cs    (Thin - Delegates to MediatR)
│   │   │   ├── OrdersController.cs
│   │   │   ├── UsersController.cs
│   │   │   └── ...
│   │   │
│   │   ├── Middleware/
│   │   │   ├── ValidationMiddleware.cs
│   │   │   ├── ExceptionMiddleware.cs
│   │   │   └── ...
│   │   │
│   │   ├── Configurations/              (Swagger, CORS, etc.)
│   │   │   ├── SwaggerConfiguration.cs
│   │   │   ├── MiddlewareConfiguration.cs
│   │   │   └── ...
│   │   │
│   │   ├── DependencyInjection.cs       (Registers Controllers, Swagger, Middleware)
│   │   ├── GlobalUsings.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   │
│   │   └── Market.API.csproj
│   │
│   └── Market.sln
│
├── tests/
│   ├── Market.Domain.Tests/
│   │   ├── UnitTest1.cs (stub)
│   │   └── Market.Domain.Tests.csproj
│   │
│   ├── Market.Application.Tests/
│   │   ├── UnitTest1.cs (stub)
│   │   └── Market.Application.Tests.csproj
│   │
│   ├── Market.Infrastructure.Tests/
│   │   ├── UnitTest1.cs (stub)
│   │   └── Market.Infrastructure.Tests.csproj
│   │
│   └── Market.API.Tests/
│       ├── UnitTest1.cs (stub)
│       └── Market.API.Tests.csproj
│
├── docs/
│   ├── ARCHITECTURE.md         (this file)
│   ├── API.md
│   ├── DOCKER.md
│   └── ...
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Layer Responsibilities

### 1. Domain Layer (`Market.Domain`)

**Purpose**: Core business models and abstractions

**Responsibilities**:
- Define business entities (Product, Category, User, Order, etc.)
- Define value objects (Slug, Money, etc.)
- Define enums (OrderStatus, UserRole, etc.)
- Define repository interfaces (contracts that Infrastructure implements)
- Encapsulate domain business rules

**Rules**:
- ✅ Can reference: Nothing (zero dependencies)
- ❌ Cannot reference: Application, Infrastructure, or API layers
- ❌ Cannot import: EF Core, MediatR, ASP.NET Core, external packages (except BCL)

**Example**:
```csharp
namespace Market.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        // ... business properties
    }
}

namespace Market.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetBySkuAsync(string sku);
    }
}
```

### 2. Application Layer (`Market.Application`)

**Purpose**: Business logic and use cases (CQRS handlers)

**Responsibilities**:
- Implement CQRS Commands (write operations)
- Implement CQRS Queries (read operations)
- Validate incoming requests (FluentValidation)
- Map between DTOs and domain models
- Orchestrate repository calls
- Define application DTOs/responses

**Rules**:
- ✅ Can reference: Domain only
- ✅ Can use: MediatR, FluentValidation, AutoMapper
- ❌ Cannot reference: Infrastructure, API layers
- ❌ Cannot import: EF Core, ASP.NET Core, controllers

**Example**:
```csharp
namespace Market.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductResponse> { }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _repo;
        
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken ct)
        {
            // Business logic using Domain interfaces
            var product = new Product(request.Name, request.Price);
            await _repo.AddAsync(product);
            return new ProductResponse { Id = product.Id, Name = product.Name };
        }
    }
}
```

### 3. Infrastructure Layer (`Market.Infrastructure`)

**Purpose**: Data access and external services

**Responsibilities**:
- Implement DbContext (SQL Server with EF Core)
- Implement repository interfaces
- Implement Unit of Work
- Configure entity mappings
- Manage database migrations
- Seed initial data
- Register IoC container

**Rules**:
- ✅ Can reference: Application, Domain
- ✅ Can use: EF Core, external libraries
- ❌ Cannot reference: API layer
- ❌ Cannot import: Controllers, HTTP concerns

**Example**:
```csharp
namespace Market.Infrastructure.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public async Task<Product> GetBySkuAsync(string sku)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.SKU == sku);
        }
    }
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddDbContext<MarketDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
```

### 4. API Layer (`Market.API`)

**Purpose**: HTTP presentation and orchestration

**Responsibilities**:
- Accept HTTP requests via Controllers
- Validate HTTP input
- Send commands/queries to MediatR
- Return HTTP responses
- Configure Swagger/OpenAPI
- Set up middleware
- Register all layer services in composition root

**Rules**:
- ✅ Can reference: Application, Infrastructure
- ✅ Can use: ASP.NET Core, MediatR
- ❌ Cannot contain: Business logic
- ❌ Cannot have: Direct repository access

**Example**:
```csharp
namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(result);
        }
    }
}

// Program.cs
builder.Services
    .AddApplicationServices()                    // Register MediatR + Validators
    .AddInfrastructureServices(config)         // Register DbContext + Repos
    .AddPresentationServices();                 // Register Controllers + Swagger
```

## Dependency Flow

```
API Layer
  ↓
  └─→ Depends on: Application + Infrastructure
  
Application Layer
  ↓
  └─→ Depends on: Domain ONLY
  
Infrastructure Layer
  ↓
  └─→ Depends on: Domain + Application
  
Domain Layer
  ↓
  └─→ Depends on: NOTHING ✓
```

**Rule**: Flow is always downward (unidirectional). No upward or sideways dependencies.

## Design Patterns

### 1. Repository Pattern
- Abstracts data access behind interfaces defined in Domain
- Each entity has a specialized repository interface
- Infrastructure implements all repository interfaces

### 2. CQRS (Command/Query Responsibility Segregation)
- Commands: Write operations (CreateProductCommand, UpdateProductCommand, etc.)
- Queries: Read operations (GetAllProductsQuery, GetProductByIdQuery, etc.)
- Handlers: IRequestHandler<TRequest, TResponse> implementations
- All routed through MediatR

### 3. Dependency Injection
- Layers register their own services via extension methods
- API composes all layers in Program.cs
- Infrastructure creates DbContext and repositories
- Application registers MediatR handlers and validators

### 4. Unit of Work
- Manages DbContext and coordinates multiple repositories
- Ensures transaction consistency
- Provides atomic operations across aggregates

### 5. Value Objects
- Encapsulate primitive values (Slug, Money, Email)
- Enforce validation rules
- Provide type safety

## SOLID Principles

### Single Responsibility
- Each class has ONE reason to change
- ProductRepository handles only Product data access
- CreateProductCommandHandler handles only product creation logic
- Controllers only orchestrate HTTP concerns

### Open/Closed
- Classes are open for extension, closed for modification
- New features added via new Commands/Queries, not modifying existing ones
- Repository interfaces allow swapping implementations

### Liskov Substitution
- All IProductRepository implementations behave identically
- Any IRepository<T> can be used interchangeably
- Handlers depend on abstractions, not concrete implementations

### Interface Segregation
- Small, focused interfaces (IProductRepository vs. IRepository<>)
- Clients don't depend on methods they don't use
- Commands/Queries are specific and single-purpose

### Dependency Inversion
- High-level modules (Application) don't depend on low-level modules (Infrastructure)
- Both depend on abstractions (Domain interfaces)
- Interfaces defined in Domain layer
- Implementations in Infrastructure layer

## Data Flow

### Command Flow (Write Operation)
```
HTTP POST
  ↓
ProductsController
  ↓
_mediator.Send(CreateProductCommand)
  ↓
CreateProductCommandHandler (Application)
  ↓
IProductRepository.AddAsync() (Domain interface)
  ↓
ProductRepository.AddAsync() (Infrastructure implementation)
  ↓
DbContext.Products.AddAsync()
  ↓
await _unitOfWork.SaveChangesAsync()
  ↓
SQL INSERT
  ↓
HTTP Response
```

### Query Flow (Read Operation)
```
HTTP GET /api/products
  ↓
ProductsController
  ↓
_mediator.Send(GetAllProductsQuery)
  ↓
GetAllProductsQueryHandler (Application)
  ↓
IProductRepository.GetAllAsync() (Domain interface)
  ↓
ProductRepository.GetAllAsync() (Infrastructure)
  ↓
DbContext.Products.ToListAsync()
  ↓
SQL SELECT
  ↓
Map to ProductResponse DTOs
  ↓
HTTP Response (JSON)
```

## Technology Stack

| Layer | Technology |
|-------|-----------|
| **Presentation** | ASP.NET Core 9.0 Web API |
| **Orchestration** | MediatR (CQRS) |
| **Validation** | FluentValidation |
| **Data Access** | Entity Framework Core 9.0 |
| **Database** | SQL Server 2022 |
| **Testing** | xUnit |
| **Language** | C# 13 (.NET 9) |
| **Build** | MSBuild |
| **Documentation** | Swagger/OpenAPI |

## Key Takeaways

1. **Strict Layer Separation**: Each layer has clear responsibilities and dependencies
2. **Domain-Centric**: Domain layer is the core; other layers serve it
3. **Pure CQRS**: All business operations modeled as Commands or Queries
4. **No Circular Dependencies**: Always flow downward
5. **Highly Testable**: Mock Domain interfaces to test Application and Infrastructure
6. **Maintainable**: New features added without modifying existing code (Open/Closed)
7. **Scalable**: Easy to add new entities, commands, queries, and handlers

---

*Last Updated: August 2026*
