# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-09-01

### Added
- **Clean Architecture Implementation**: Strict 4-layer architecture (Domain → Application → Infrastructure → API)
- **SOLID Principles**: Applied throughout the codebase for maintainability and scalability
- **Pure CQRS Pattern**: Complete Command Query Responsibility Segregation implementation
- **SQL Server Integration**: Migrated from MongoDB to SQL Server with Entity Framework Core 9.0.19
- **Repository Pattern**: Generic repository with specific implementations for all entities
- **Unit of Work Pattern**: Centralized transaction management
- **MediatR 12.0.1**: Request/response messaging for decoupled handlers
- **Dependency Injection**: Comprehensive DI configuration across all layers
- **Swagger Documentation**: Auto-generated API documentation

### Changed
- **Infrastructure Layer Reorganization**: 
  - `Data/` folder: DbContext, configurations, seeders
  - `Persistence/` folder: Repositories, UnitOfWork, migrations
- **Project Structure**: Migrated to `/src` and `/tests` directories for clean organization
- **Namespace Updates**: Corrected all namespaces to reflect 4-layer architecture
- **Dockerfile Optimization**: Build only production-needed source projects (excludes tests)

### Fixed
- **Package Downgrade Error**: Updated `Microsoft.Extensions.DependencyInjection.Abstractions` to 10.0.0 in Infrastructure layer
- **Docker Build Cache**: Invalidated stale NuGet cache for fresh dependency resolution
- **Test Projects**: Moved test projects to separate `/tests` folder with correct references
- **Old MongoDB References**: Removed all MongoDB connection strings and legacy configurations

### Removed
- **Monolithic Structure**: Deleted old `Market.API.sln` file
- **MongoDB Integration**: Removed MongoDB driver and configuration
- **Legacy Project Layout**: Removed root-level project folders in favor of `/src` and `/tests`
- **Old Migrations**: Cleaned up migrations from API layer (now in Infrastructure)

### Technical Details

#### Dependencies Updated
- MediatR: 12.0.1 (stable)
- Microsoft.EntityFrameworkCore: 9.0.19
- Microsoft.EntityFrameworkCore.SqlServer: 9.0.19
- Microsoft.Extensions.DependencyInjection.Abstractions: 10.0.0
- Microsoft.Extensions.Logging.Abstractions: 10.0.0
- BCrypt.Net-Next: 4.0.3

#### Build & Deployment
- .NET SDK: 9.0
- Target Framework: net9.0
- Docker: Multi-stage build (build → publish → runtime)
- CI/CD: GitHub Actions with Docker build and push
- Registry: GHCR (GitHub Container Registry)

#### Architecture Layers
1. **Domain Layer** (Market.Domain): Entities, value objects, interfaces, business logic
2. **Application Layer** (Market.Application): DTOs, commands, queries, handlers, validators
3. **Infrastructure Layer** (Market.Infrastructure): DbContext, repositories, migrations, UoW
4. **API Layer** (Market.API): Controllers, middleware, dependency injection, Swagger

---

**Status**: Production Ready
**Breaking Changes**: Yes (MongoDB → SQL Server migration)
**Database Migration Required**: Yes

