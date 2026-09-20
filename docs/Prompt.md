
# Market API — Architecture Refactoring & Boundary Enforcement Prompt

## Role

You are acting as a Senior Software Architect and Principal .NET Engineer.

You are working on:

**Repository:** `Mostafa-SAID7/Market-Api`

The goal is NOT to cosmetically reorganize folders.

The goal is to make the codebase architecturally consistent, maintainable, testable, scalable, and ready for future evolution into a modular monolith and, where justified, selective microservices.

---

# 1. Primary Architecture Decision

Use the following architecture:

```text
Modular Monolith
+
Clean Architecture
+
Vertical Slice Architecture
+
CQRS
+
DDD-lite / Aggregate-oriented Domain Model
+
EF Core
+
SQL Server
```

Do NOT convert the application to Microservices at this stage.

Do NOT introduce unnecessary abstractions or projects.

Optimize for:

* clear ownership
* explicit boundaries
* low coupling
* high cohesion
* testability
* scalability
* maintainability
* production readiness

---

# 2. Mandatory Dependency Rules

The dependency direction must be:

```text
API
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Application abstractions
 ↓
Domain
```

Allowed:

```text
Domain → nothing external

Application → Domain

Infrastructure → Application + Domain

API → Application + Infrastructure
```

Forbidden:

```text
Domain → Application
Domain → Infrastructure
Domain → API

Application → Infrastructure

Infrastructure → API

Infrastructure → Application.Features.* DTOs

Infrastructure → Controllers

Application → ASP.NET HTTP concerns
```

The API may reference Infrastructure only because it is the Composition Root.

Infrastructure must never depend on Application feature implementations or API contracts.

---

# 3. First Perform an Architecture Audit

Before changing code, inspect the entire repository.

Inspect:

```text
src/
tests/
docs/
*.csproj
*.sln
Program.cs
DependencyInjection.cs
EF configurations
Repositories
UnitOfWork
CQRS handlers
Validators
Middleware
Controllers
Migrations
Docker
CI/CD
configuration
```

Create an internal architecture map containing:

1. project dependencies
2. namespace dependencies
3. repository dependencies
4. CQRS dependencies
5. database dependencies
6. HTTP dependencies
7. cross-layer violations
8. duplicate abstractions
9. duplicate responsibilities
10. transaction boundaries

Do not modify anything until this audit is complete.

---

# 4. Fix Unit of Work Duplication

There are currently duplicate Unit of Work abstractions.

Find all:

```text
IUnitOfWork
UnitOfWork
```

There must be ONE authoritative abstraction.

Do not keep both:

```text
Market.Domain.Repositories.IUnitOfWork
```

and:

```text
Market.Infrastructure.Persistence.UnitOfWork.IUnitOfWork
```

Choose one clean architecture strategy.

Preferred:

```text
Market.Application
└── Abstractions
    └── Persistence
        └── IUnitOfWork.cs
```

Infrastructure implements it.

Ensure dependency injection registers the SAME interface consumed by Application handlers.

Add tests that verify DI resolution.

---

# 5. Fix Repository Transaction Violations

Repositories must NOT call:

```csharp
SaveChangesAsync()
SaveChanges()
BeginTransaction()
CommitTransaction()
RollbackTransaction()
```

inside normal CRUD methods.

Current repository behavior must be refactored.

Repository responsibilities:

```text
Get
Find
Add
Update
Remove
Exists
```

Application/use-case responsibilities:

```text
business operation
transaction boundary
SaveChanges
```

There must be exactly one persistence commit per application command unless the use case explicitly requires multiple transaction boundaries.

---

# 6. Correct EF Core Unit of Work Semantics

Remember:

```text
DbContext already behaves as Unit of Work.
```

Do not create a useless abstraction over DbContext merely for pattern compliance.

If an explicit UnitOfWork abstraction is retained, it must provide real application-level value.

Prefer:

```text
IUnitOfWork
└── SaveChangesAsync()
```

rather than exposing every repository through the UnitOfWork if direct repository injection provides a cleaner boundary.

Evaluate both approaches and choose the simpler production-ready one.

Do not keep an abstraction only because the project historically used it.

---

# 7. Remove Infrastructure → Application Feature Dependencies

Find all Infrastructure classes referencing:

```text
Market.Application.Features.*
```

Especially:

```text
ProductResponse
CreateProductCommand
Query DTOs
Handlers
Validators
```

Infrastructure must NOT depend on Application feature DTOs.

For read operations create:

```text
Market.Application
└── Abstractions
    └── Persistence
        ├── IProductReadRepository.cs
        ├── IOrderReadRepository.cs
        └── ...
```

Infrastructure implements those abstractions.

Read repositories may perform SQL-side projections.

---

# 8. Separate Read and Write Responsibilities

Do not force all CQRS operations through generic CRUD repositories.

For write operations:

```text
Command
 ↓
Handler
 ↓
Aggregate Repository
 ↓
Domain Aggregate
 ↓
SaveChanges
```

For read operations:

```text
Query
 ↓
Handler
 ↓
Read Repository / Query Service
 ↓
SQL projection
 ↓
DTO
```

Read queries should:

* use `AsNoTracking()` where appropriate
* project directly to DTOs
* filter in SQL
* paginate in SQL
* avoid unnecessary entity materialization
* avoid loading large navigation graphs

---

# 9. Implement Pagination

Any endpoint returning collections must be reviewed.

Remove unbounded:

```csharp
GetAllAsync()
ToListAsync()
```

patterns from public collection endpoints.

Introduce a standard pagination abstraction.

At minimum support:

```text
page
pageSize
totalCount
items
```

For high-volume feed-style endpoints evaluate cursor pagination.

Do not introduce cursor pagination everywhere.

Use it only where it provides measurable value.

---

# 10. Protect Aggregate Boundaries

Review:

```text
Product
Order
OrderItem
Cart
CartItem
Vendor
User
Review
```

Identify aggregate roots.

Recommended direction:

```text
Product → Aggregate Root

Order → Aggregate Root
  └── OrderItem

Cart → Aggregate Root
  └── CartItem

Vendor → Aggregate Root

User → Aggregate Root
```

Avoid unrestricted public setters for business-critical state.

Prefer methods such as:

```csharp
product.ChangePrice(...)
product.IncreaseStock(...)
product.DecreaseStock(...)
product.Activate()
product.Deactivate()

order.AddItem(...)
order.RemoveItem(...)
order.CalculateTotals()
order.Confirm()
order.Cancel()
order.MarkAsPaid()
order.MarkAsShipped()

cart.AddItem(...)
cart.RemoveItem(...)
cart.UpdateQuantity(...)
cart.Clear()
```

Business invariants must be enforced inside the domain model.

---

# 11. Protect Inventory from Concurrency Problems

Review:

```text
Product.Quantity
Product.Sold
CartItem.Quantity
Order creation
```

Implement an explicit concurrency strategy.

Preferred SQL Server strategy:

```text
rowversion
+
EF Core optimistic concurrency
```

Handle:

```csharp
DbUpdateConcurrencyException
```

appropriately.

Do not silently overwrite inventory changes.

Add tests for concurrent updates.

---

# 12. Order Pricing Rules

Review:

```text
Product.Price
CartItem.Price
OrderItem.Price
Order.SubTotal
Order.TotalPrice
Tax
Shipping
Discount
```

Ensure OrderItem stores the purchase-time price snapshot.

Do not calculate historical order prices from the current Product price.

Define ownership clearly.

The Order must remain historically correct even if:

```text
Product.Price
Product.Name
Product.DiscountPrice
```

change later.

---

# 13. Domain Model Improvements

Do not turn every property into a private setter blindly.

Only protect business-critical state.

Use:

```text
private setters
factory methods
domain methods
value objects
domain validation
```

where appropriate.

Review existing:

```text
Money
Slug
Rating
Tag
```

Value Objects.

Ensure they are actually used where they provide business value.

Do not introduce Value Objects only for cosmetic purposes.

---

# 14. Remove Duplicate Validation Responsibility

Validation must be owned by the Application pipeline.

Preferred:

```text
HTTP Request
 ↓
Controller
 ↓
MediatR
 ↓
ValidationBehavior
 ↓
Handler
```

Create:

```text
Application/Behaviors/ValidationBehavior.cs
```

Use FluentValidation validators registered from Application.

Remove or simplify:

```text
ValidationMiddleware
```

if it duplicates Application validation.

HTTP middleware should handle HTTP concerns, not application command validation.

---

# 15. Add Useful MediatR Behaviors

Evaluate and implement only the behaviors that provide real value.

Possible:

```text
ValidationBehavior
PerformanceBehavior
LoggingBehavior
TransactionBehavior
```

Do NOT create behaviors that duplicate Serilog, ASP.NET authorization, or other existing infrastructure.

TransactionBehavior should apply only where appropriate.

Queries normally should not start database write transactions.

Commands that require atomic persistence should use a transaction boundary.

---

# 16. Replace Custom Exception Response With ProblemDetails

Remove sensitive exception details from API responses.

Do NOT return:

```csharp
exception.Message
```

to clients for internal exceptions.

Use ASP.NET Core ProblemDetails.

Production responses should resemble:

```json
{
  "type": "https://api.market.com/errors/internal",
  "title": "An unexpected error occurred.",
  "status": 500,
  "traceId": "..."
}
```

Detailed exception information belongs in logs.

Create a consistent mapping for:

```text
Validation
NotFound
Conflict
Unauthorized
Forbidden
Concurrency
Business rule violations
Unexpected errors
```

---

# 17. Correlation and Observability

Review:

```text
CorrelationIdMiddleware
RequestLoggingMiddleware
Serilog
HealthChecks
```

Avoid implementing duplicate tracing mechanisms.

Prefer:

```text
Activity.TraceId
OpenTelemetry-ready tracing
structured Serilog logging
```

Correlation identifiers should be available to:

```text
logs
responses
diagnostics
downstream calls
```

Do not log:

```text
passwords
JWT tokens
authorization headers
sensitive customer information
payment secrets
```

---

# 18. Middleware Ordering

Establish an explicit production pipeline.

Target concept:

```text
Exception handling
 ↓
Correlation / tracing
 ↓
Request logging
 ↓
HTTPS
 ↓
Static files where required
 ↓
Routing
 ↓
Authentication
 ↓
Authorization
 ↓
Endpoints
```

Do not place authorization without authentication.

Verify whether the project currently has:

```text
UseAuthentication()
```

If JWT authentication exists, ensure:

```text
AddAuthentication()
AddAuthorization()
UseAuthentication()
UseAuthorization()
```

are correctly configured and ordered.

---

# 19. Authentication Boundary

Infrastructure may implement authentication-related services.

Application must depend only on abstractions such as:

```text
ICurrentUser
IPasswordHasher
ITokenService
```

Do not put JWT implementation details inside Domain.

Do not expose JWT infrastructure to Domain entities.

Review:

```text
BCrypt
JWT
User authentication
Roles
Authorization
```

and enforce correct ownership.

---

# 20. Database Initialization

Do not automatically perform production migrations and seeding during every application startup.

Review:

```text
InitializeDatabaseAsync()
MigrateAsync()
DataSeeder
```

Preferred production strategy:

```text
CI/CD
 ↓
Database migration job
 ↓
Application deployment
```

Application startup may optionally validate schema state but should not become the migration orchestrator for multiple production replicas.

Seed development/test data separately from production migrations.

---

# 21. Soft Delete

Centralize soft-delete behavior.

Avoid repeating:

```csharp
.Where(x => !x.IsDeleted)
```

everywhere.

Evaluate EF Core global query filters.

Provide an explicit administrative strategy for:

```text
IgnoreQueryFilters()
restore
hard delete
```

Do not accidentally expose deleted records.

---

# 22. Database Constraints and Indexes

Review all EF Core configurations.

Ensure appropriate indexes and constraints exist for:

```text
User.Email
Product.SKU
Order.OrderNumber
Vendor.UserId
Cart.UserId
Product.CategoryId
Product.VendorId
Order.CustomerId
Review.ProductId
Review.UserId
```

Do not add indexes blindly.

For every important index, identify:

```text
query pattern
selectivity
write cost
uniqueness
sort/filter usage
```

Add composite indexes where actual query patterns require them.

---

# 23. Domain Events and Future Messaging

Do not add RabbitMQ simply because the project is called scalable.

Prepare the architecture for domain events.

Potential events:

```text
ProductCreated
ProductStockChanged
OrderPlaced
OrderPaid
OrderCancelled
ReviewCreated
VendorApproved
```

Keep domain events independent from:

```text
RabbitMQ
Azure Service Bus
HTTP
Kafka
```

If external messaging is introduced later, use:

```text
Domain Event
 ↓
Outbox
 ↓
Message Broker
 ↓
Consumers
```

Do not publish external messages directly from Domain entities.

---

# 24. Modular Monolith Boundaries

Reorganize business ownership conceptually around:

```text
Catalog
├── Products
├── Categories
└── Tags

Ordering
├── Cart
├── Orders
└── OrderItems

Identity
└── Users

Marketplace
└── Vendors

Reviews
└── Reviews
```

Do not necessarily create separate .NET projects for every module.

Use namespaces/folders to enforce business ownership.

The architecture must make future extraction possible.

---

# 25. Future Microservice Extraction

Do not create microservices now.

Instead make the following boundaries explicit:

```text
Catalog
Ordering
Identity
Marketplace
Reviews
```

For each module document:

```text
owned entities
owned data
public commands
public queries
events emitted
events consumed
external dependencies
transaction boundaries
```

A future microservice extraction must not require rewriting the entire business logic.

---

# 26. API Contracts

Controllers must remain thin.

Controllers may:

```text
bind HTTP input
send MediatR request
return HTTP response
```

Controllers must NOT:

```text
access DbContext
access repositories directly
contain business rules
calculate order totals
manage transactions
hash passwords
modify aggregates directly
```

Review every controller.

---

# 27. DTO Boundaries

Separate:

```text
API request DTO
Application command/query
Domain entity
Database model
API response DTO
```

Do not reuse Domain entities as HTTP contracts.

Do not let Infrastructure return API response models.

Do not create mapping layers unnecessarily when direct projections are clearer.

---

# 28. Remove CQRS Abuse

CQRS must represent meaningful command/query separation.

Avoid unnecessary chains such as:

```text
Handler
 ↓
Mediator.Send()
 ↓
Internal Command
 ↓
Another Handler
```

If a command handler can execute its use case directly, do so.

Do not use MediatR recursively just to create extra classes.

---

# 29. Logging

Avoid logging every handler with redundant messages when structured request logging already provides the information.

Logs should answer:

```text
What happened?
Where?
When?
Correlation/Trace ID?
User?
Duration?
Business operation?
Failure reason?
```

Do not log sensitive information.

Use appropriate log levels.

---

# 30. Performance

Review all database access for:

```text
N+1 queries
tracking
AsNoTracking
projection
pagination
unbounded queries
unnecessary Includes
large object materialization
duplicate queries
connection lifetime
async I/O
```

Do not optimize based on assumptions.

Keep hot-path queries database-efficient.

---

# 31. Caching Strategy

Prepare abstractions for:

```text
ICacheService
```

but do not cache everything.

Good candidates:

```text
Product catalog
Categories
Public vendor data
Popular products
```

Dangerous candidates without consistency strategy:

```text
Inventory
Cart
Order
Payment
```

If Redis is introduced, define:

```text
TTL
key format
invalidation
stampede protection
serialization
failure behavior
```

---

# 32. Testing Strategy

The current test projects must not remain placeholders.

Create meaningful tests.

## Domain

Test:

```text
aggregate invariants
value objects
state transitions
business calculations
invalid states
```

## Application

Test:

```text
commands
queries
validators
behaviors
authorization
transaction behavior
```

## Infrastructure

Test:

```text
EF mappings
repositories
constraints
indexes where practical
concurrency
database integration
```

## API

Test:

```text
HTTP status codes
authentication
authorization
validation
ProblemDetails
endpoint behavior
```

Use integration tests with a realistic SQL Server strategy where appropriate.

---

# 33. Architecture Tests

Add automated architecture tests preventing:

```text
Domain → Infrastructure
Domain → Application
Domain → API
Application → Infrastructure
Infrastructure → API
Infrastructure → Application.Features
```

Architecture tests must fail the build when boundaries are violated.

---

# 34. Package and Framework Alignment

Audit every `.csproj`.

Ensure:

```text
TargetFramework
EF Core
ASP.NET Core packages
MediatR
FluentValidation
Swagger
Serilog
HealthChecks
```

are compatible.

The repository currently targets .NET 9.

Evaluate migration to .NET 10 LTS before finalizing the refactor.

Do not blindly upgrade all packages.

Check compatibility and test the entire solution.

---

# 35. Docker and CI/CD

Review:

```text
Dockerfile
docker-compose.yml
GitHub Actions
database migration
tests
security scanning
CodeQL
Dependabot
release pipeline
```

CI must execute:

```text
restore
build
test
architecture tests
format
security checks
docker build
```

Production database migrations should be separated from API startup where appropriate.

---

# 36. Documentation

After refactoring, update:

```text
README.md
docs/ARCHITECTURE.md
docs/PROJECT-STRUCTURE.md
docs/IMPLEMENTATION.md
```

Documentation must describe the ACTUAL architecture.

Never document architecture that the code does not implement.

---

# 37. Required Target Structure

Aim for approximately:

```text
src/
├── Market.Domain/
│   ├── Common/
│   ├── Catalog/
│   ├── Ordering/
│   ├── Identity/
│   ├── Marketplace/
│   └── Reviews/
│
├── Market.Application/
│   ├── Abstractions/
│   │   ├── Persistence/
│   │   ├── Identity/
│   │   ├── Caching/
│   │   └── Messaging/
│   ├── Behaviors/
│   └── Features/
│       ├── Catalog/
│       ├── Ordering/
│       ├── Identity/
│       ├── Marketplace/
│       └── Reviews/
│
├── Market.Infrastructure/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   ├── Repositories/
│   │   ├── Queries/
│   │   └── Migrations/
│   ├── Identity/
│   ├── Caching/
│   └── Messaging/
│
└── Market.API/
    ├── Controllers/
    ├── Contracts/
    ├── Middleware/
    └── Program.cs
```

Do not blindly move every existing file.

Move files according to responsibility and dependency ownership.

---

# 38. Refactoring Process

Perform the work in this order:

## Phase 1 — Audit

No behavior changes.

Document:

```text
current dependencies
violations
duplicates
transaction boundaries
business boundaries
```

## Phase 2 — Dependency correction

Fix:

```text
duplicate interfaces
wrong project references
Infrastructure → Application feature references
```

## Phase 3 — Persistence correction

Fix:

```text
Repository SaveChanges
UnitOfWork
read/write separation
query projections
pagination
concurrency
```

## Phase 4 — Application pipeline

Implement:

```text
ValidationBehavior
required behaviors
clean command/query boundaries
```

## Phase 5 — Domain

Protect:

```text
aggregate invariants
state transitions
pricing
inventory
order lifecycle
```

## Phase 6 — API

Fix:

```text
middleware
ProblemDetails
authentication
authorization
controllers
contracts
```

## Phase 7 — Testing

Implement:

```text
unit tests
integration tests
architecture tests
concurrency tests
```

## Phase 8 — Production readiness

Review:

```text
Docker
CI/CD
migrations
observability
security
configuration
```

## Phase 9 — Documentation

Update all architecture documentation to match reality.

---

# 39. Critical Acceptance Criteria

The refactor is NOT complete unless all are true.

### Architecture

* [ ] No forbidden project dependencies
* [ ] No duplicate architectural abstractions
* [ ] Domain has no Infrastructure dependency
* [ ] Application has no Infrastructure dependency
* [ ] Infrastructure does not depend on Application feature DTOs
* [ ] API is the composition root

### Persistence

* [ ] Repositories do not call SaveChanges
* [ ] One clear transaction boundary exists
* [ ] UnitOfWork is not duplicated
* [ ] Read queries use projections
* [ ] Collection endpoints are paginated
* [ ] Concurrency is explicitly handled

### Domain

* [ ] Aggregates have clear ownership
* [ ] Business invariants are protected
* [ ] Order pricing is historically correct
* [ ] Inventory cannot be corrupted by simple concurrent requests
* [ ] State transitions are explicit

### Application

* [ ] CQRS boundaries are meaningful
* [ ] No unnecessary MediatR-to-MediatR chains
* [ ] Validation is in the application pipeline
* [ ] Behaviors do not duplicate middleware responsibilities

### API

* [ ] Controllers are thin
* [ ] No direct DbContext usage
* [ ] No repository usage from controllers
* [ ] Authentication and authorization pipeline is correct
* [ ] ProblemDetails is used consistently
* [ ] Internal exception details are never exposed

### Testing

* [ ] Domain tests exist
* [ ] Application tests exist
* [ ] Infrastructure integration tests exist
* [ ] API integration tests exist
* [ ] Architecture tests exist
* [ ] Concurrency tests exist for inventory/order-sensitive operations

### Production

* [ ] No production secrets committed
* [ ] Database migrations are deployment-safe
* [ ] Logging is structured
* [ ] Trace/correlation IDs work
* [ ] Health checks work
* [ ] CI runs build + test + architecture tests
* [ ] Docker build works

---

# 40. Verification Commands

Before finishing:

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
dotnet format --verify-no-changes
```

If architecture tests exist:

```bash
dotnet test --filter Category=Architecture
```

Also verify:

```text
docker build
docker compose
database migration
API startup
Swagger
health endpoint
authentication
authorization
```

---

# 41. Final Report Required From the Agent

After implementation, provide:

## Architecture Summary

```text
Architecture:
Dependency direction:
Business modules:
Aggregate roots:
Persistence strategy:
CQRS strategy:
Transaction strategy:
Concurrency strategy:
Caching strategy:
Messaging strategy:
Scaling strategy:
```

## Changed Files

List every important moved/created/deleted file.

## Removed Duplications

Explicitly list:

```text
duplicate interfaces
duplicate middleware responsibilities
duplicate transaction handling
duplicate validation
duplicate DTO ownership
```

## Remaining Risks

List anything intentionally not changed and explain why.

## Verification

Report exact results of:

```text
build
tests
architecture tests
format
docker
database
```

Do not claim success without actually verifying it.

---

# 42. Important Constraints

DO NOT:

* rewrite the entire project unnecessarily
* introduce microservices
* introduce RabbitMQ without a real use case
* introduce Redis without a cache strategy
* create interfaces for every class
* create generic abstractions without business value
* expose IQueryable from Application
* expose DbContext to Controllers
* make Domain depend on EF Core
* move business logic into Infrastructure
* use Infrastructure DTOs inside Application
* use HTTP concepts inside Domain
* use MediatR recursively without a real reason
* hide architecture violations by namespace tricks

Prefer the smallest architecture that provides strong boundaries.

The final system must be understandable by a senior engineer during a code review.

The objective is not "more architecture".

The objective is:

```text
Clear Boundaries
+
Correct Ownership
+
Predictable Transactions
+
Strong Domain Rules
+
Efficient Queries
+
Safe Concurrency
+
Testable Use Cases
+
Production Scalability
```
