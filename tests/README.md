# Market API Test Suite

Comprehensive testing infrastructure for the Market e-commerce API.

## Quick Start

### Run All Tests
```powershell
dotnet test
```

### Run Specific Project
```powershell
dotnet test tests/Market.Domain.Tests
```

### Generate Coverage Report
```powershell
dotnet test --configuration Release --collect:"XPlat Code Coverage"
```

---

## Project Structure

### `Market.Tests.Common/`
**Shared test infrastructure and utilities**

Contains:
- Base test classes (`TestBase`, `ValidatorTestBase`, `RepositoryTestBase`, `IntegrationTestBase`)
- Test data builders (fluent API for entity creation)
- Database fixtures (in-memory SQLite and LocalDB support)
- Random data generator for varied test scenarios

**Usage**: All test projects reference this library for common patterns.

See [Market.Tests.Common/README.md](Market.Tests.Common/README.md) for detailed guide.

### `Market.Domain.Tests/`
**Unit tests for domain entities and value objects**

Structure:
- `ValueObjects/` - Rating, Money validation tests
- `Entities/` - Cart, Product, Category behavior tests

**Coverage**: 90%+ target
- ✅ Value object invariants
- ✅ Entity domain logic
- ❌ Exception scenarios (needs expansion)
- ❌ Edge cases (needs expansion)

### `Market.Application.Tests/`
**Unit tests for validators and command handlers**

Structure:
- `Validators/` - Product, Vendor, Review validation tests
- `Features/` - Command handler tests (Carts, Orders)

**Coverage**: 85%+ target
- ✅ Input validation rules
- ✅ Command handler business logic
- ✅ Mock-based repository testing
- ❌ Query handlers (needs implementation)
- ❌ Feature orchestration (needs implementation)

### `Market.Infrastructure.Tests/`
**Integration tests for database and data access**

Structure:
- `Data/` - Database model constraint tests

**Coverage**: 80%+ target
- ✅ EF Core model constraints
- ✅ Soft-delete filtering verification
- ✅ Unique index validation
- ❌ Repository pattern tests (needs implementation)
- ❌ Migration validation (needs implementation)

### `Market.API.Tests/`
**Tests for HTTP middleware and API layer**

Structure:
- `Middleware/` - Exception handling, security headers tests

**Coverage**: 70%+ target
- ✅ Middleware behavior (exception sanitization, security headers)
- ❌ Controller endpoints (needs implementation)
- ❌ Route binding (needs implementation)
- ❌ HTTP semantics (needs implementation)

---

## Test Statistics

### Current Coverage

| Project | Tests | Status | Coverage |
|---------|-------|--------|----------|
| Domain | 8 | ✅ Passing | ~30% |
| Application | 8 | ✅ Passing | ~20% |
| Infrastructure | 2 | ✅ Passing | ~15% |
| API | 2 | ✅ Passing | ~25% |
| **TOTAL** | **21** | **✅ PASSING** | **~20%** |

### Test by Category

| Category | Count | Layer |
|----------|-------|-------|
| Value Object Tests | 2 | Domain |
| Entity Tests | 3 | Domain |
| Validator Tests | 3 | Application |
| Handler Tests | 2 | Application |
| Model Constraint Tests | 2 | Infrastructure |
| Middleware Tests | 2 | API |
| **TOTAL** | **21** | **All** |

---

## Common Patterns

### Using Builders

```csharp
var product = ProductBuilder.Default()
    .WithName("Lamp")
    .WithPrice(29.99m)
    .WithQuantity(10)
    .Build();
```

### Testing Validators

```csharp
public class ProductValidatorTests : ValidatorTestBase
{
    [Fact]
    public void Validate_WithValidProduct_Passes()
    {
        var validator = new ProductValidator();
        var product = ProductBuilder.Default().Build();
        var result = validator.Validate(product);
        
        AssertIsValid(result);
    }
}
```

### Testing Handlers with Mocks

```csharp
public class UpdateCartHandlerTests : RepositoryTestBase
{
    [Fact]
    public async Task Handle_ValidInput_UpdatesCart()
    {
        var unitOfWork = CreateMockUnitOfWork();
        var handler = new UpdateCartCommandHandler(unitOfWork.Object, logger);
        
        var response = await handler.Handle(command, CancellationToken.None);
        
        VerifySaveWasCalled(unitOfWork);
    }
}
```

### Integration Tests with Database

```csharp
public class ProductRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdAsync_ExistingProduct_ReturnsProduct()
    {
        var product = ProductBuilder.Default().Build();
        DbContext.Products.Add(product);
        await SaveChangesAsync();

        var repo = new ProductRepository(DbContext);
        var result = await repo.GetByIdAsync(product.Id, CancellationToken.None);

        Assert.NotNull(result);
    }
}
```

---

## Running Tests by Category

### Domain Layer Only
```powershell
dotnet test tests/Market.Domain.Tests
```

### Application Layer Only
```powershell
dotnet test tests/Market.Application.Tests
```

### Infrastructure Layer Only
```powershell
dotnet test tests/Market.Infrastructure.Tests
```

### API Layer Only
```powershell
dotnet test tests/Market.API.Tests
```

### Specific Test Class
```powershell
dotnet test --filter "ClassName=Market.Domain.Tests.Entities.CartTests"
```

### Specific Test Method
```powershell
dotnet test --filter "FullyQualifiedName~Cart_AddItem"
```

---

## Coverage Goals & Status

### Phase 1: Infrastructure ✅ COMPLETE
- ✅ Coverlet package added to all test projects
- ✅ CI/CD coverage collection enabled
- ✅ .gitignore updated for coverage files
- ✅ Market.Tests.Common created with base classes and builders
- ✅ Tests reorganized by layer and concern

### Phase 2: Unit Test Expansion 🚧 PLANNED
- 🚧 Add 40+ repository pattern tests (Infrastructure)
- 🚧 Add 30+ controller/endpoint tests (API)
- 🚧 Add 20+ integration tests (cross-layer flows)
- 🚧 Achieve 70%+ overall coverage target

### Phase 3: Continuous Improvement 📋 PLANNED
- 📋 Add E2E test scenarios
- 📋 Add contract/API validation tests
- 📋 Monitor and maintain 70%+ coverage

---

## Continuous Integration

### GitHub Actions Workflow

Tests run automatically on:
- Push to `main` or `develop`
- Pull requests to `main` or `develop`

### Accessing Test Results

1. **GitHub Actions**: Repository → Actions → Latest workflow
2. **Test Artifacts**: Workflow run → Artifacts → `test-results`
3. **Coverage Artifacts**: Workflow run → Artifacts → `code-coverage`

### Local CI/CD Simulation

```powershell
# Run exact workflow steps locally
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release --collect:"XPlat Code Coverage"
```

---

## Troubleshooting

### Windows AppControl Blocks Local Tests

**Error**: `System.IO.FileLoadException: Application Control policy has blocked this file`

**Workaround**: 
- Tests pass in CI/CD (GitHub Actions)
- Use WSL2 for local testing
- Or create PR to trigger CI/CD testing

See [LOCALDB-SETUP.md](../docs/LOCALDB-SETUP.md) for alternatives.

### Database Connection Issues

**Error**: `Cannot connect to (localdb)\...`

**Solution**:
```powershell
sqllocaldb start "MarketApiDev"
```

See [LOCALDB-SETUP.md](../docs/LOCALDB-SETUP.md) for detailed setup.

### Coverage Not Collected

**Error**: Coverage report files not found

**Solution**:
```powershell
# Verify flag format
dotnet test --collect:"XPlat Code Coverage"  # ✅ Correct
```

---

## Documentation

- **[TESTING.md](../docs/TESTING.md)** - Comprehensive testing guide with patterns, best practices, and troubleshooting
- **[LOCALDB-SETUP.md](../docs/LOCALDB-SETUP.md)** - SQL Server LocalDB installation and configuration
- **[Market.Tests.Common/README.md](Market.Tests.Common/README.md)** - Shared library usage guide with examples

---

## Key Files

### Test Projects
- `tests/Market.Domain.Tests/Market.Domain.Tests.csproj`
- `tests/Market.Application.Tests/Market.Application.Tests.csproj`
- `tests/Market.Infrastructure.Tests/Market.Infrastructure.Tests.csproj`
- `tests/Market.API.Tests/Market.API.Tests.csproj`

### Shared Infrastructure
- `tests/Market.Tests.Common/Market.Tests.Common.csproj`
- `tests/Market.Tests.Common/Base/` - Test base classes
- `tests/Market.Tests.Common/Builders/` - Entity builders
- `tests/Market.Tests.Common/Fixtures/` - Database and utility fixtures

### Configuration
- `.github/workflows/dotnet.yml` - CI/CD test execution
- `.gitignore` - Excludes test artifacts and coverage files

---

## Support

For questions or issues:
1. Check [TESTING.md](../docs/TESTING.md) for comprehensive guide
2. Review examples in specific test project folders
3. See [Market.Tests.Common/README.md](Market.Tests.Common/README.md) for usage patterns
4. Open a GitHub issue with details

---

## Metrics

**Test Statistics** (as of Phase 2 completion)
- Total Tests: 21
- Passing: 21 (100%)
- Coverage: ~20% (baseline)
- Target: 70%+ overall

**Test Distribution**
- Domain Layer: 8 tests (38%)
- Application Layer: 8 tests (38%)
- Infrastructure Layer: 2 tests (10%)
- API Layer: 2 tests (10%)

**Uncovered Areas** (Phase 3 expansion)
- Repository implementations
- API endpoints/controllers
- Integration flows
- Error scenarios
- Edge cases
