# Market.Tests.Common

Shared test utilities and infrastructure for the Market API test suite.

## Contents

### Base Classes

- **`TestBase`** - Foundation for all unit tests
  - Mock creation helpers (`CreateStrictMock`, `CreateLooseMock`)
  - Common setup and teardown logic

- **`ValidatorTestBase`** - Specialized base for validator tests
  - Assertion helpers (`AssertHasError`, `AssertIsValid`)
  - Simplified validation testing

- **`RepositoryTestBase`** - Specialized base for data access tests
  - Repository mock creation
  - Unit of work helpers
  - Verification utilities

### Fixtures

- **`DbContextFixture`** - Database context management
  - In-memory SQLite for fast tests
  - SQL LocalDB option for integration tests
  - Automatic cleanup

- **`RandomDataFixture`** - Random test data generation
  - Prices, quantities, ratings
  - Emails, IDs, strings
  - Used for varied test scenarios

### Builders (See `Builders/` directory)

Entity builders for creating test objects with fluent API:
- `UserBuilder`
- `ProductBuilder`
- `CartBuilder`
- `CartItemBuilder`
- `OrderBuilder`
- `OrderItemBuilder`
- `VendorBuilder`
- `ReviewBuilder`
- `CategoryBuilder`

## Usage Examples

### Using TestBase

```csharp
public class MyTests : TestBase
{
    [Fact]
    public void MyTest()
    {
        var mockRepo = CreateStrictMock<IProductRepository>();
        // ... setup and assertions
    }
}
```

### Using ValidatorTestBase

```csharp
public class ProductValidatorTests : ValidatorTestBase
{
    [Fact]
    public void ValidProduct_Passes()
    {
        var validator = new ProductValidator();
        var product = ProductBuilder.Default().Build();
        var result = validator.Validate(product);
        
        AssertIsValid(result);
    }

    [Fact]
    public void InvalidPrice_Fails()
    {
        var validator = new ProductValidator();
        var product = ProductBuilder.Default().WithPrice(-10m).Build();
        var result = validator.Validate(product);
        
        AssertIsInvalid(result);
        AssertHasError(result.Errors, nameof(Product.Price));
    }
}
```

### Using DbContextFixture

```csharp
public class ProductRepositoryTests : IDisposable
{
    private readonly DbContextFixture _fixture;

    public ProductRepositoryTests()
    {
        _fixture = DbContextFixture.CreateInMemory();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingProduct_ReturnsProduct()
    {
        var context = _fixture.GetContext();
        var product = ProductBuilder.Default().Build();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var result = await repo.GetByIdAsync(product.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    public void Dispose() => _fixture.Dispose();
}
```

### Using Entity Builders

```csharp
// Simple usage
var product = ProductBuilder.Default().Build();

// Custom values
var product = ProductBuilder.Default()
    .WithName("Premium Lamp")
    .WithPrice(99.99m)
    .WithQuantity(50)
    .WithStatus(ProductStatus.Active)
    .Build();

// Collection building
var cart = CartBuilder.Default()
    .WithItems(new[]
    {
        CartItemBuilder.Default().WithProductId(1).Build(),
        CartItemBuilder.Default().WithProductId(2).Build()
    })
    .Build();
```

## Integration with Test Projects

Each test project references this library:

```xml
<ProjectReference Include="../../tests/Market.Tests.Common/Market.Tests.Common.csproj" />
```

Then add to test class:

```csharp
using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;
using Market.Tests.Common.Fixtures;
```

## Coverage

This library provides:
- ✅ Consistent test infrastructure across all layers
- ✅ Reduced boilerplate in individual test files
- ✅ Reusable test data builders
- ✅ Database fixture management
- ✅ Mock management utilities
