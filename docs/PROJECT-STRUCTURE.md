# Project Structure

Complete directory structure and organization of Market API v2.0+

## Root Directory Structure

```
Market-Api/
├── .git/                          # Git repository
├── .github/                       # GitHub configuration
│   ├── workflows/                 # CI/CD workflows
│   │   ├── docker.yml
│   │   └── dotnet.yml
│   ├── ISSUE_TEMPLATE/           # Issue templates
│   │   ├── bug_report.md
│   │   └── feature_request.md
│   ├── CODE_OF_CONDUCT.md
│   ├── PULL_REQUEST_TEMPLATE.md
│   └── dependabot.yml
├── .gitignore                    # Git ignore rules
├── Market.API/                   # Main application folder
├── docs/                         # Documentation (see below)
├── LICENSE                       # MIT License
├── README.md                     # Project overview
├── docker-compose.yml            # Docker Compose configuration
├── Dockerfile                    # Docker image definition
└── CHANGELOG.md                  # Version history

```

## Market.API Directory Structure

```
Market.API/
├── bin/                          # Compiled binaries (auto-generated)
├── obj/                          # Object files (auto-generated)
│
├── Common/                       # Shared utilities
│   └── BaseEntity.cs            # Base class for all entities
│
├── Configurations/               # Dependency Injection & Setup
│   ├── DataConfiguration.cs     # Repository & UnitOfWork DI
│   ├── MediatRConfiguration.cs  # MediatR handler registration
│   ├── ServiceConfiguration.cs  # Service layer DI
│   ├── ValidatorConfiguration.cs # Validator DI
│   ├── MiddlewareConfiguration.cs # Pipeline setup
│   └── SwaggerConfiguration.cs  # Swagger/OpenAPI setup
│
├── Controllers/                  # HTTP Request Handlers
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── UsersController.cs
│   ├── VendorsController.cs
│   ├── OrdersController.cs
│   ├── CartsController.cs
│   └── ReviewsController.cs
│
├── Data/                         # Data Access Layer
│   ├── MongoDbContext.cs        # MongoDB connection & indexes
│   ├── Interfaces/              # Repository contracts
│   │   ├── IRepository.cs       # Generic repository interface
│   │   ├── IProductRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   ├── IUserRepository.cs
│   │   ├── IVendorRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── ICartRepository.cs
│   │   └── IReviewRepository.cs
│   ├── Repositories/            # Repository implementations
│   │   ├── Repository.cs        # Generic repository base
│   │   ├── ProductRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── VendorRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── CartRepository.cs
│   │   └── ReviewRepository.cs
│   ├── UnitOfWork/              # Coordinated repository access
│   │   ├── IUnitOfWork.cs      # UnitOfWork interface
│   │   └── UnitOfWork.cs       # UnitOfWork implementation
│   ├── Seeds/                   # Sample data
│   │   └── DataSeeder.cs
│   └── indexes/                 # MongoDB index definitions
│
├── Features/                     # MediatR Commands & Queries
│   ├── Products/
│   │   ├── Commands/            # Write operations
│   │   │   ├── CreateProductCommand.cs
│   │   │   ├── UpdateProductCommand.cs
│   │   │   └── DeleteProductCommand.cs
│   │   ├── Queries/             # Read operations
│   │   │   ├── GetAllProductsQuery.cs
│   │   │   ├── GetProductByIdQuery.cs
│   │   │   └── GetProductsByCategoryQuery.cs
│   │   └── ProductResponse.cs   # Response DTO
│   │
│   ├── Categories/
│   │   ├── Commands/
│   │   │   ├── CreateCategoryCommand.cs
│   │   │   ├── UpdateCategoryCommand.cs
│   │   │   └── DeleteCategoryCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetAllCategoriesQuery.cs
│   │   │   └── GetCategoryByIdQuery.cs
│   │   └── CategoryResponse.cs
│   │
│   ├── Users/
│   │   ├── Commands/
│   │   │   ├── CreateUserCommand.cs
│   │   │   ├── UpdateUserCommand.cs
│   │   │   └── DeleteUserCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetAllUsersQuery.cs
│   │   │   └── GetUserByIdQuery.cs
│   │   └── UserResponse.cs
│   │
│   ├── Vendors/
│   │   ├── Commands/
│   │   │   ├── CreateVendorCommand.cs
│   │   │   ├── UpdateVendorCommand.cs
│   │   │   ├── DeleteVendorCommand.cs
│   │   │   └── ApproveVendorCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetAllVendorsQuery.cs
│   │   │   └── GetVendorByIdQuery.cs
│   │   └── VendorResponse.cs
│   │
│   ├── Orders/
│   │   ├── Commands/
│   │   │   ├── CreateOrderCommand.cs
│   │   │   ├── UpdateOrderCommand.cs
│   │   │   └── DeleteOrderCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetAllOrdersQuery.cs
│   │   │   └── GetOrderByIdQuery.cs
│   │   └── OrderResponse.cs
│   │
│   ├── Carts/
│   │   ├── Commands/
│   │   │   ├── AddToCartCommand.cs
│   │   │   ├── RemoveFromCartCommand.cs
│   │   │   └── ClearCartCommand.cs
│   │   ├── Queries/
│   │   │   └── GetCartByUserIdQuery.cs
│   │   └── CartResponse.cs
│   │
│   ├── Reviews/
│   │   ├── Commands/
│   │   │   ├── CreateReviewCommand.cs
│   │   │   ├── UpdateReviewCommand.cs
│   │   │   └── DeleteReviewCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetAllReviewsQuery.cs
│   │   │   └── GetReviewsByProductIdQuery.cs
│   │   └── ReviewResponse.cs
│   │
│   └── MEDIATOR_STRUCTURE.md   # MediatR structure documentation
│
├── Middleware/                   # HTTP Pipeline Middleware
│   └── ValidationMiddleware.cs  # Validation exception handler
│
├── Models/
│   ├── Entities/                # Domain models
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── User.cs
│   │   ├── Vendor.cs
│   │   ├── Order.cs
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Review.cs
│   │   ├── OrderItem.cs
│   │   └── BaseEntity.cs
│   ├── Enums/                   # Enumeration types
│   │   ├── OrderStatus.cs
│   │   ├── PaymentStatus.cs
│   │   └── UserRole.cs
│   └── ValueObjects/            # Value objects
│       └── Slug.cs
│
├── Services/                     # Business Logic Layer
│   ├── ProductService.cs
│   ├── CategoryService.cs
│   ├── UserService.cs
│   ├── VendorService.cs
│   ├── OrderService.cs
│   ├── CartService.cs
│   ├── ReviewService.cs
│   └── Interfaces/              # Service contracts
│       ├── IProductService.cs
│       ├── ICategoryService.cs
│       ├── IUserService.cs
│       ├── IVendorService.cs
│       ├── IOrderService.cs
│       ├── ICartService.cs
│       └── IReviewService.cs
│
├── Settings/                     # Configuration models
│   └── MongoDbSettings.cs       # MongoDB connection settings
│
├── Validators/                   # Validation framework
│   ├── IValidator.cs            # Validator interface
│   ├── ValidationResult.cs      # Validation result DTO
│   ├── ValidationError.cs       # Validation error DTO
│   ├── ProductValidator.cs
│   ├── CategoryValidator.cs
│   ├── UserValidator.cs
│   ├── VendorValidator.cs
│   ├── OrderValidator.cs
│   ├── CartValidator.cs
│   └── ReviewValidator.cs
│
├── Program.cs                    # Application entry point
├── appsettings.json             # Configuration (local)
├── appsettings.Development.json # Development configuration
├── Market.API.csproj            # Project file
└── Properties/
    └── launchSettings.json      # Launch profiles

```

## Docs Directory Structure

```
docs/
├── API.md                    # REST API endpoint reference
├── ARCHITECTURE.md           # System design & patterns
├── CHANGELOG.md              # Version history
├── CONTRIBUTING.md           # Contribution guidelines
├── DOCKER.md                 # Docker setup guide
├── DOCUMENTATION.md          # Navigation guide (master index)
├── IMPLEMENTATION.md         # Code patterns & examples
└── PROJECT-STRUCTURE.md      # This file (directory structure)

```

## .github Directory Structure

```
.github/
├── workflows/
│   ├── docker.yml           # Docker build & push workflow
│   └── dotnet.yml           # .NET build & test workflow
├── ISSUE_TEMPLATE/
│   ├── bug_report.md        # Bug report template
│   └── feature_request.md   # Feature request template
├── CODE_OF_CONDUCT.md       # Community code of conduct
├── PULL_REQUEST_TEMPLATE.md # PR template
└── dependabot.yml           # Dependency update automation

```

## Detailed Layer Descriptions

### 1. Controllers Layer (`Controllers/`)
- HTTP request handlers
- Route definitions
- Response formatting
- Delegates to MediatR

**Pattern**: One controller per entity

### 2. MediatR Layer (`Features/`)
- Command handlers (Create, Update, Delete)
- Query handlers (Get, Filter)
- Response DTOs
- Request/Response mapping

**Pattern**: `Entity/Commands/` and `Entity/Queries/` organization

### 3. Service Layer (`Services/`)
- Business logic
- Validation
- Logging
- Cross-cutting concerns

**Pattern**: Interface-first design with dependency injection

### 4. Repository Layer (`Data/Repositories/`)
- Data access abstraction
- Generic CRUD operations
- Entity-specific queries
- MongoDB operations

**Pattern**: Generic repository + specific repositories

### 5. Data Layer (`Data/`)
- Entity definitions (in `Models/Entities/`)
- MongoDB context
- Index creation
- Connection management

**Pattern**: Unit of Work coordinating repositories

### 6. Validation Layer (`Validators/`)
- Entity validation rules
- Custom validation logic
- Error messages

**Pattern**: Validator interface implementation

### 7. Configuration Layer (`Configurations/`)
- Dependency injection setup
- Middleware configuration
- Service registration

**Pattern**: Extension methods for fluent setup

## Key Organizational Principles

### 1. **By Feature**
- Each entity has its own folder in `Features/`
- Related commands and queries grouped together
- Reduces complexity for large projects

### 2. **By Layer**
- Controllers, Services, Repositories in separate folders
- Clear separation of concerns
- Easy to locate code by responsibility

### 3. **By Type**
- Commands, Queries, DTOs separated
- Middleware, Validators in dedicated folders
- Configuration organized centrally

## File Naming Conventions

| Type | Naming Pattern | Example |
|------|---|---|
| Controllers | `[Entity]Controller.cs` | `ProductsController.cs` |
| Services | `[Entity]Service.cs` | `ProductService.cs` |
| Service Interfaces | `I[Entity]Service.cs` | `IProductService.cs` |
| Repositories | `[Entity]Repository.cs` | `ProductRepository.cs` |
| Repository Interfaces | `I[Entity]Repository.cs` | `IProductRepository.cs` |
| Commands | `[Action][Entity]Command.cs` | `CreateProductCommand.cs` |
| Queries | `Get[Criteria][Entity]Query.cs` | `GetProductByIdQuery.cs` |
| DTOs | `[Entity]Response.cs` | `ProductResponse.cs` |
| Validators | `[Entity]Validator.cs` | `ProductValidator.cs` |
| Middleware | `[Name]Middleware.cs` | `ValidationMiddleware.cs` |
| Configurations | `[Category]Configuration.cs` | `ServiceConfiguration.cs` |

## Common File Locations

### To find a specific feature:
```
Features/[Entity]/Commands/Create[Entity]Command.cs
Features/[Entity]/Queries/Get[Entity]Query.cs
Features/[Entity]/[Entity]Response.cs
```

### To find business logic:
```
Services/[Entity]Service.cs
Services/Interfaces/I[Entity]Service.cs
```

### To find data access:
```
Data/Repositories/[Entity]Repository.cs
Data/Interfaces/I[Entity]Repository.cs
```

### To find validation:
```
Validators/[Entity]Validator.cs
```

### To find HTTP routes:
```
Controllers/[Entity]Controller.cs
```

## Build Output

After building, these directories are generated:

```
Market.API/
├── bin/
│   ├── Debug/
│   │   └── net9.0/          # Debug build output
│   └── Release/             # Release build output
├── obj/                     # Intermediate build files
└── Properties/
    └── PublishProfiles/     # Publishing configurations
```

## Package & Project Files

- **Market.API.csproj** - C# project file with dependencies
- **appsettings.json** - Default configuration
- **appsettings.Development.json** - Development-specific config
- **appsettings.Production.json** - Production-specific config

## Docker Structure

```
Repository Root/
├── Dockerfile              # Multi-stage build for .NET app
├── docker-compose.yml      # Compose configuration
│   ├── market-api service  # ASP.NET Core API
│   └── mongodb service     # MongoDB database
└── .dockerignore           # Docker exclusions
```

## Documentation Structure

All user-facing documentation is in `/docs/`:
- API reference
- Architecture guide
- Implementation guide
- Contribution guidelines
- Docker/deployment guide
- Project structure (this file)

See **[docs/DOCUMENTATION.md](DOCUMENTATION.md)** for navigation guide.

## Summary Statistics

| Category | Count |
|----------|-------|
| Entity Models | 8 |
| Controllers | 7 |
| Services | 7 |
| Repositories | 7 |
| Validators | 7 |
| Commands | 19+ |
| Queries | 12+ |
| Middleware | 1 |
| Configurations | 6 |
| Documentation Files | 8 |

---

For navigation and understanding where to find things, refer to **[docs/DOCUMENTATION.md](DOCUMENTATION.md)** or the quick table above.
