# Testing Architecture & Guidelines

Complete guide to the Market API testing infrastructure, patterns, and best practices.

## Table of Contents

1. [Overview](#overview)
2. [Test Structure](#test-structure)
3. [Running Tests](#running-tests)
4. [Writing Tests](#writing-tests)
5. [Test Patterns](#test-patterns)
6. [Code Coverage](#code-coverage)
7. [CI/CD Integration](#cicd-integration)
8. [Troubleshooting](#troubleshooting)

---

## Overview

### Testing Philosophy

The Market API follows a **layered testing strategy**:

- **Unit Tests** (Fast, isolated, mocked dependencies)
  - Domain layer: Entity behavior, value objects
  - Application layer: Validators, handlers
  - API layer: Middleware

- **Integration Tests** (Real database, multiple components)
  - Repository patterns
  - Database constraints and migrations
  - End-to-end feature flows

- **Coverage Goal**: 70%+ line coverage, 85%+ on critical paths

### Test Framework

- **Framework:** xUnit 2.9.3
- **Mocking:** Moq 4.20.72
- **Coverage:** Coverlet 6.0.0
- **Database:** SQLite (in-memory) or SQL Server LocalDB

---

## Test Structure

### Directory Layout

```
tests/
├── Market.Tests.Common/              # Shared test infrastructure
│   ├── Base/                         # Base test classes
│   │   ├── TestBase.cs              # Foundation for all tests
│   │   ├── ValidatorTestBase.cs     # For validator tests
│   │   ├── RepositoryTestBase.cs    # For data access tests
│   │   └── IntegrationTestBase.cs   # For integration tests
│   ├── Builders/                     # Test data builders
│   │   ├── UserBuilder.cs
│   │   ├── ProductBuilder.cs
│   │   ├── CartBuilder.cs
│   │   └── ... (one for each entity)
│   ├── Fixtures/                     # Test infrastructure
│   │   ├── DbContextFixture.cs      # Database management
│   │   ├── IntegrationTestFixture.cs
│   │   └── RandomDataFixture.cs
│   └── README.md                     # Detailed usage guide
│
├── Market.Domain.Tests/
│   ├── ValueObjects/                 # Value object tests
│   │   ├── RatingTests.cs
│   │   └── MoneyTests.cs
│   ├── Entities/                     # Entity behavior tests
│   │   ├── CartTests.cs
│   │   ├── ProductTests.cs
│   │   └── CategoryTests.cs
│   └── Market.Domain.Tests.csproj
│
├── Market.Application.Tests/
│   ├── Validators/                   # Input validation tests
│   │   ├── ProductValidatorTests.cs
│   │   ├── VendorValidatorTests.cs
│   │   └── ReviewValidatorTests.cs
│   ├── Features/                     # Feature handler tests
│   │   ├── Carts/
│   │   │   └── UpdateCartItemQuantityHandlerTests.cs
│   │   └── Orders/
│   │       └── CreateOrderHandlerTests.cs
│   └── Market.Application.Tests.csproj
│
├── Market.Infrastructure.Tests/
│   ├── Data/                         # Database tests
│   │   └── DatabaseModelTests.cs
│   └── Market.Infrastructure.Tests.csproj
│
└── Market.API.Tests/
    ├── Middleware/                   # Middleware tests
    │   ├── ExceptionHandlingMiddlewareTests.cs
    │   └── SecurityHeadersMiddlewareTests.cs
    └── Market.API.Tests.csproj
```

### Naming Conventions

| Aspect | Convention | Example |
|--------|-----------|---------|
| **Test Class** | `[Subject]Tests` | `ProductValidatorTests` |
| **Test Method** | `[Method]_[Scenario]_[Expected]` | `Validate_WithInvalidPrice_FailsValidation` |
| **Builder Class** | `[Entity]Builder` | `ProductBuilder` |
| **Builder Method** | `With[Property]` or `[Scenario]` | `WithPrice`, `ForUser` |

---

## Running Tests

### Run All Tests

```powershell
dotnet test
```

### Run Specific Project

```powershell
dotnet test tests/Market.Domain.Tests
```

### Run Specific Class

```powershell
dotnet test --filter "ClassName=Market.Domain.Tests.Entities.CartTests"
```

### Run with Coverage

```powershell
dotnet test --configuration Release --collect:"XPlat Code Coverage"
```

### Run in Release Mode (Faster)

```powershell
dotnet test --configuration Release --verbosity minimal
```

### Watch Mode (Requires manual invocation)

For development, run tests after each change:

```powershell
# In PowerShell, run tests in a loop
while ($true) { dotnet test; Read-Host "Press Enter to re-run..." }
```

---

## Writing Tests

### Basic Unit Test

```csharp
using Market.Tests.Common.Base;

public class ProductValidatorTests : ValidatorTestBase
{
    [Fact]
    public void Validate_WithValidProduct_Passes()
    {
        // Arrange
        var validator = new ProductValidator();
        var product = ProductBuilder.Default()
            .WithPrice(29.99m)
            .WithQuantity(10)
            .Build();

        // Act
        var result = validator.Validate(product);

        // Assert
        AssertIsValid(result);
    }

    [Fact]
    public void Validate_WithNegativePrice_Fails()
    {
        var validator = new ProductValidator();
        var product = ProductBuilder.Default()
            .WithPrice(-10m)
            .Build();

        var result = validator.Validate(product);

        AssertIsInvalid(result);
        AssertHasError(result.Errors, nameof(Product.Price));
    }
}
```

### Testing with Mocks (Repository Pattern)

```csharp
using Market.Tests.Common.Base;

public class CreateOrderHandlerTests : RepositoryTestBase
{
    [Fact]
    public async Task Handle_UsesPersistedPrice_IgnoresInput()
    {
        // Arrange
        var product = new Product { Id = 1, Price = 20m, DiscountPrice = 15m };
        
        var products = CreateMockRepository<IProductRepository>();
        products.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        products.Setup(x => x.UpdateAsync(product, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var unitOfWork = CreateMockUnitOfWork();
        unitOfWork.SetupGet(x => x.Products).Returns(products.Object);
        unitOfWork.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderCommandHandler(unitOfWork.Object, NullLogger<CreateOrderCommandHandler>.Instance);

        // Act
        var response = await handler.Handle(
            new CreateOrderCommand { Items = [new OrderItemInput { ProductId = 1, Price = 0.01m }] },
            CancellationToken.None);

        // Assert
        Assert.Equal(15m, response.Items.First().Price);  // Uses persisted price
        VerifySaveWasCalled(unitOfWork);
    }
}
```

### Integration Tests with Database

```csharp
using Market.Tests.Common.Base;

public class ProductRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetByIdAsync_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = ProductBuilder.Default().WithName("Lamp").Build();
        DbContext.Products.Add(product);
        await SaveChangesAsync();

        var repo = new ProductRepository(DbContext);

        // Act
        var result = await repo.GetByIdAsync(product.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Lamp", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonexistentProduct_ReturnsNull()
    {
        var repo = new ProductRepository(DbContext);

        var result = await repo.GetByIdAsync(9999, CancellationToken.None);

        Assert.Null(result);
    }
}
```

---

## Test Patterns

### Theory Tests (Parameterized)

```csharp
[Theory]
[InlineData(1)]
[InlineData(5)]
public void Rating_Create_AcceptsValidBoundaries(int value)
{
    var rating = Rating.Create(value);
    Assert.Equal(value, rating.Value);
}
```

### Testing for Exceptions

```csharp
[Fact]
public void Rating_Create_WithInvalidValue_ThrowsArgumentException()
{
    Assert.Throws<ArgumentException>(() => Rating.Create(0));
    Assert.Throws<ArgumentException>(() => Rating.Create(6));
}
```

### Async Tests

```csharp
[Fact]
public async Task UpdateCartItemQuantityHandler_ValidQuantity_UpdatesCart()
{
    var handler = new UpdateCartItemQuantityCommandHandler(unitOfWork.Object, logger);

    var response = await handler.Handle(
        new UpdateCartItemQuantityCommand { Quantity = 5 },
        CancellationToken.None);

    Assert.NotNull(response);
}
```

### Testing Multiple Assertions

```csharp
[Fact]
public void Order_Creation_SetsAllFields()
{
    var order = OrderBuilder.Default()
        .WithCustomerId(1)
        .WithSubtotal(100m)
        .WithTax(10m)
        .Build();

    Assert.Equal(1, order.CustomerId);
    Assert.Equal(100m, order.SubTotal);
    Assert.Equal(10m, order.Tax);
    Assert.Equal(OrderStatus.Pending, order.Status);
}
```

---

## Code Coverage

### Generate Coverage Report

```powershell
# Collect coverage
dotnet test --configuration Release --collect:"XPlat Code Coverage"

# Coverage files are saved to: **/coverage.cobertura.xml
```

### View Coverage Metrics

Coverage reports are uploaded to GitHub Actions artifacts on each PR/push. Check:
1. PR → Checks → Details → Artifacts
2. Run logs for coverage percentage by project

### Coverage Targets

| Layer | Target | Priority |
|-------|--------|----------|
| Domain | 90%+ | Critical |
| Application | 85%+ | High |
| Infrastructure | 80%+ | Medium |
| API (Controllers) | 70%+ | Medium |
| **Overall** | **70%+** | **Baseline** |

### Improving Coverage

1. **Identify gaps**: Check coverage report for untested lines
2. **Write tests**: Add tests for uncovered scenarios
3. **Test helpers**: Use builders and fixtures to simplify test setup
4. **Run locally**: Verify coverage before pushing:
   ```powershell
   dotnet test --configuration Release --collect:"XPlat Code Coverage"
   ```

---

## CI/CD Integration

### GitHub Actions Workflow

Tests run automatically on:
- ✅ Push to `main` or `develop`
- ✅ Pull requests to `main` or `develop`

### Workflow Steps

1. **Checkout** code
2. **Setup .NET 9.0**
3. **Restore** dependencies
4. **Build** (Release configuration)
5. **Run Tests** with coverage collection
6. **Upload** test results & coverage artifacts
7. **Publish** API (if tests pass)

### Monitoring CI/CD

- View test results: GitHub Actions → Latest workflow run
- Download artifacts: Workflow run → Artifacts section
- Check coverage: Look for `code-coverage` artifact

### Status Badges

Add to README.md:
```markdown
![Tests](https://github.com/YOUR_ORG/Market-Api/actions/workflows/dotnet.yml/badge.svg)
```

---

## Troubleshooting

### Tests Won't Run Locally (Windows AppControl)

**Error**: `Application Control policy has blocked this file`

**Solution**: Tests pass on CI/CD. For local development, use WSL2 or Docker:

```powershell
# WSL2
wsl dotnet test

# Or run via CI/CD by pushing branch
git push origin feature-branch
```

### Test Database Locked

**Error**: `Database is in use` or `Cannot create database`

**Solution**:
```powershell
# Stop SQL Server LocalDB
sqllocaldb stop "MarketApiDev"

# Or delete old test databases
sqlcmd -S "(localdb)\MarketApiDev" -Q "DROP DATABASE MarketDb_OldTest"
```

### Tests Timeout

**Cause**: Slow database setup or network issues

**Solution**:
```powershell
# Use in-memory SQLite instead of LocalDB
# Or increase timeout: dotnet test --logger "console;verbosity=detailed"
```

### Mock Setup Failures

**Error**: `Mock invocation not matched` or `Unexpected call`

**Solution**: Verify mock setup matches actual calls:
```csharp
// Ensure mock setup includes CancellationToken
carts.Setup(x => x.GetByUserIdAsync(9, It.IsAny<CancellationToken>()))
    .ReturnsAsync(cart);

// Use MockBehavior.Strict to catch unsetup calls
var mock = new Mock<IRepository>(MockBehavior.Strict);
```

### Builder Syntax Errors

**Error**: `'WithXxx' does not exist`

**Solution**: Builders follow `With[PropertyName]` pattern:
```csharp
// Correct
ProductBuilder.Default().WithPrice(29.99m).Build()

// Incorrect (no "Set" prefix)
ProductBuilder.Default().SetPrice(29.99m).Build()  // ❌
```

### Coverage Not Collected

**Error**: Coverage reports not generated

**Solution**:
```powershell
# Ensure flag is correct
dotnet test --collect:"XPlat Code Coverage"  # ✅

# Not: --collect:Coverage (old format)
dotnet test --collect:Coverage              # ❌
```

---

## Best Practices

### ✅ DO

- Use builders for consistent test data creation
- Test one concern per test method
- Mock external dependencies (repositories, services)
- Use descriptive test names
- Keep tests independent (no shared state)
- Use `Assert.Single()` when expecting one item
- Test both success and failure paths

### ❌ DON'T

- Don't create hard-coded test data (use builders)
- Don't test multiple concerns in one test
- Don't skip testing error scenarios
- Don't use ambiguous names (`Test1`, `TestPass`)
- Don't share test state between methods
- Don't write integration tests without isolation
- Don't commit test artifacts (TestResults/, coverage/)

---

## Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/Moq/moq4)
- [SQL Server LocalDB Setup](./LOCALDB-SETUP.md)
- [Test Common Library](../tests/Market.Tests.Common/README.md)
- [Phase 1 Audit Report](./ARCHITECTURE.md)

---

## Support & Questions

For issues or improvements to the testing infrastructure:
1. Check this documentation first
2. Review test examples in specific project folders
3. Consult Phase 1 audit findings
4. Create a GitHub issue with details
