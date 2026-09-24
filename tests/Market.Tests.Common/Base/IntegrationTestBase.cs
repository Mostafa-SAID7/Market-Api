namespace Market.Tests.Common.Base;

using Microsoft.EntityFrameworkCore;
using Xunit;

/// <summary>
/// Base class for integration tests that require a real or in-memory database.
/// Provides database context management and common setup/teardown.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected Fixtures.DbContextFixture DbFixture { get; private set; } = null!;
    protected MarketDbContext DbContext => DbFixture.GetContext();

    /// <summary>
    /// Override to customize database fixture creation.
    /// Default: Creates in-memory SQLite database.
    /// </summary>
    protected virtual Fixtures.DbContextFixture CreateDbFixture()
        => Fixtures.DbContextFixture.CreateInMemory();

    /// <summary>
    /// Called once before any tests in the class run.
    /// </summary>
    public virtual Task InitializeAsync()
    {
        DbFixture = CreateDbFixture();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called once after all tests in the class have run.
    /// </summary>
    public virtual Task DisposeAsync()
    {
        DbFixture?.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Clears all data from the database for test isolation.
    /// Call this in test setup if you need a clean database per test.
    /// </summary>
    protected async Task ClearDatabaseAsync()
        => await DbFixture.ClearDatabaseAsync();

    /// <summary>
    /// Completely resets the database schema.
    /// Use sparingly as it's slower than ClearDatabaseAsync.
    /// </summary>
    protected async Task ResetDatabaseAsync()
        => await DbFixture.ResetDatabaseAsync();

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    protected async Task SaveChangesAsync()
        => await DbContext.SaveChangesAsync();

    /// <summary>
    /// Reloads an entity from the database to verify persistence.
    /// Useful for asserting that changes were actually saved.
    /// </summary>
    protected async Task<T?> ReloadEntityAsync<T>(T entity) where T : class
    {
        DbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return await DbContext.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == EF.Property<int>(entity, "Id"));
    }

    /// <summary>
    /// Asserts that an entity exists in the database.
    /// </summary>
    protected async Task<T> AssertEntityExistsAsync<T>(int id) where T : class
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        Assert.NotNull(entity);
        return entity;
    }

    /// <summary>
    /// Asserts that an entity does not exist in the database.
    /// </summary>
    protected async Task AssertEntityDoesNotExistAsync<T>(int id) where T : class
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        Assert.Null(entity);
    }
}
