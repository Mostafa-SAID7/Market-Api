namespace Market.Tests.Common.Fixtures;

/// <summary>
/// Collection fixture for sharing database context across test classes in a collection.
/// Implements ICollectionFixture to allow multiple test classes to share the same database instance.
/// </summary>
[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
    // This class has no code, it's just used to define the collection
}

/// <summary>
/// Fixture that manages a shared database context for integration test collections.
/// Use this when you want multiple test classes to share the same database state.
/// </summary>
public class IntegrationTestFixture : IAsyncLifetime
{
    private DbContextFixture? _dbFixture;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    public MarketDbContext DbContext => _dbFixture?.GetContext()
        ?? throw new InvalidOperationException("Fixture not initialized");

    /// <summary>
    /// Initializes the shared database context.
    /// </summary>
    public async Task InitializeAsync()
    {
        _dbFixture = DbContextFixture.CreateInMemory();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Cleans up the shared database context.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (_dbFixture != null)
        {
            await _dbFixture.DisposeAsync();
        }
    }

    /// <summary>
    /// Clears all data from the database for test isolation.
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        if (_dbFixture != null)
        {
            await _dbFixture.ClearDatabaseAsync();
        }
    }

    /// <summary>
    /// Resets the entire database schema.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        if (_dbFixture != null)
        {
            await _dbFixture.ResetDatabaseAsync();
        }
    }
}
