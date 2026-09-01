<div align="center">

# 🛒 Market API

**A High-Performance, Enterprise-Grade E-Commerce REST API with Clean Architecture**

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=for-the-badge)](LICENSE)

*Built with strict 4-layer Clean Architecture, CQRS (MediatR), and SQL Server for maximum scalability, testability, and maintainability.*

</div>

---

## 🎯 Key Highlights

*   **⚡ High Performance**: Powered by **.NET 9** and **SQL Server** with optimized query patterns.
*   **🏗️ Clean Architecture**: Strict **4-layer separation** (Domain → Application → Infrastructure → API) with zero cross-layer dependencies.
*   **⚙️ Pure CQRS**: Complete Command/Query segregation using **MediatR** with full command/query handlers.
*   **🛡️ SOLID Principles**: Dependency Inversion, Interface Segregation, Single Responsibility enforced at all layers.
*   **🧪 Testable Design**: Fully mockable repositories, services, and handlers with dedicated test projects for each layer.
*   **🐳 Container Ready**: Fully configured for **Docker** and **Docker Compose** with SQL Server support.
*   **🤖 CI/CD Automated**: Protected by GitHub Actions (CodeQL, Dependabot, Semantic Releases, and PR Linters).

---

## 📁 Project Structure

```
Market-Api/
├── src/
│   ├── Market.Domain/              # Entities, Enums, Value Objects, Repository Interfaces (no external deps)
│   ├── Market.Application/         # Commands, Queries, Handlers, DTOs, Validators (depends only on Domain)
│   ├── Market.Infrastructure/      # DbContext, EF Core Configs, Repository Implementations, Seeds (depends on Application & Domain)
│   └── Market.API/                 # Controllers, Middleware, Program.cs (thin presentation layer)
├── tests/
│   ├── Market.Domain.Tests/        # Domain entity and value object tests
│   ├── Market.Application.Tests/   # CQRS handler and validator tests
│   ├── Market.Infrastructure.Tests/# Repository and data access tests
│   └── Market.API.Tests/           # Integration and endpoint tests
├── Market.sln                      # Solution file
├── Dockerfile
├── docker-compose.yml
└── docs/                           # Comprehensive documentation
```

---

## 🚀 Getting Started

### 1. Prerequisites
*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   **SQL Server** (LocalDB, Express, or Docker)

### 2. Quick Setup

```bash
# Clone the repository
git clone https://github.com/Mostafa-SAID7/Market-Api.git
cd Market-Api

# Restore packages
dotnet restore Market.sln
```

### 3. Database Configuration

Update your SQL Server connection string in `src/Market.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MarketApiDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### 4. Run the API

```bash
# Build the solution
dotnet build Market.sln

# Run the API from the src/Market.API directory
cd src/Market.API
dotnet run
```
*   **Swagger UI**: `https://localhost:7294/swagger`

### 5. Run with Docker Compose

```bash
# Build and run all services (API + SQL Server)
docker-compose up --build

# Access API at http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

---

## 🏛️ 4-Layer Clean Architecture

### 1. **Domain Layer** (`Market.Domain`)
- **Entities**: Product, Category, User, Vendor, Order, Cart, Review
- **Enums**: OrderStatus, PaymentStatus, UserRole, ProductStatus, VendorApprovalStatus
- **Value Objects**: Slug, Money, Email
- **Repository Interfaces**: IProductRepository, IOrderRepository, IUnitOfWork, etc.
- **Zero external dependencies** — only C# BCL

### 2. **Application Layer** (`Market.Application`)
- **CQRS Handlers**: Command/Query handlers for all business operations
- **Features**: Organized by feature (Products, Orders, Users, Vendors, Reviews, Carts, Categories)
- **DTOs/Responses**: Request and response models for API contracts
- **Validators**: FluentValidation rules for commands and queries
- **Dependency Inversion**: Depends on Domain interfaces only; Infrastructure implements

### 3. **Infrastructure Layer** (`Market.Infrastructure`)
- **DbContext**: `MarketDbContext` configured for SQL Server
- **Entity Configurations**: EF Core fluent configurations for all entities
- **Repository Implementations**: Concrete repository classes implementing Domain interfaces
- **Data Seeding**: Initial data seeds for development/testing
- **Migrations**: EF Core migrations for schema management
- **Dependency Registration**: Extension methods for IoC container setup

### 4. **API Layer** (`Market.API`)
- **Controllers**: Thin, validation-only endpoints that delegate to MediatR
- **Middleware**: Cross-cutting concerns (exception handling, logging, CORS)
- **Program.cs**: Composition root with layer dependency injection
- **Swagger/OpenAPI**: Auto-generated API documentation
- **Zero business logic** — only orchestration and presentation

---

## 🧪 Testing

```bash
# Run all tests
dotnet test Market.sln

# Run specific layer tests
dotnet test tests/Market.Domain.Tests/
dotnet test tests/Market.Application.Tests/
dotnet test tests/Market.Infrastructure.Tests/
dotnet test tests/Market.API.Tests/
```

---

## 📚 Documentation & Reference

Looking to dive deeper? Check out our dedicated documentation:

*   [**API Reference**](docs/API.md) - Endpoints for Products, Orders, Users, Vendors, etc.
*   [**Architecture Guide**](docs/ARCHITECTURE.md) - Detailed 4-layer design, dependencies, and patterns.
*   [**Docker Setup**](docs/DOCKER.md) - Container deployment and configuration.
*   [**Implementation Details**](docs/IMPLEMENTATION.md) - Design decisions and technical specifications.
*   [**Contribution Guidelines**](docs/CONTRIBUTING.md) - How to contribute and coding standards.

---

## 🔄 CQRS & MediatR Pattern

All business operations follow the **Command/Query Responsibility Segregation** pattern:

```csharp
// Commands (Write Operations)
public class CreateProductCommand : IRequest<ProductResponse> { ... }
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse> { ... }

// Queries (Read Operations)
public class GetProductByIdQuery : IRequest<ProductResponse> { ... }
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse> { ... }
```

---

## 🗄️ Database

**SQL Server** with **Entity Framework Core 9.0**:
- Fully normalized relational schema
- Fluent API configuration for all entities
- Automatic migrations support
- Seed data for development
- Connection pooling and optimized queries

---

<div align="center">
  <p>Built with ❤️ by <a href="https://github.com/Mostafa-SAID7">Mostafa SAID</a></p>
</div>
