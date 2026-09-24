using Microsoft.EntityFrameworkCore;

namespace Market.Tests.Common.Fixtures;

/// <summary>
/// Fixture for managing test database contexts.
/// Provides both in-memory and SQL LocalDB options for integration tests.
/// </summary>
public class DbContextFixture : IDisposable, IAsyncDisposable
{
    private MarketDbContext? _context;
    private readonly bool _isInMemory;

    private DbContextFixture(MarketDbContext context, bool isInMemory)
    {
        _context = context;
        _isInMemory = isInMemory;
    }

    /// <summary>
    /// Creates an in-memory SQLite database context for fast unit/integration tests.
    /// Database is created fresh for each test and cleaned up after.
    /// </summary>
    public static DbContextFixture CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<MarketDbContext>()
            .UseSqlite("Data Source=:memory:")
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new MarketDbContext(options);
        context.Database.EnsureCreated();
        return new DbContextFixture(context, isInMemory: true);
    }

    /// <summary>
    /// Creates a SQL Server LocalDB context for integration tests.
    /// Database is created fresh for each test session.
    /// </summary>
    public static DbContextFixture CreateLocalDb(string databaseName)
    {
        var connectionString = $"Server=(localdb)\\mssqllocaldb;Database={databaseName};Trusted_Connection=True;";
        var options = new DbContextOptionsBuilder<MarketDbContext>()
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new MarketDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return new DbContextFixture(context, isInMemory: false);
    }

    /// <summary>
    /// Gets the underlying database context for test queries and assertions.
    /// </summary>
    public MarketDbContext GetContext()
        => _context ?? throw new InvalidOperationException("Context has been disposed");

    /// <summary>
    /// Clears all entities from the database without deleting it.
    /// Useful for test isolation between individual test methods.
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        if (_context == null) return;

        // Delete data in order of foreign key dependencies
        await _context.ReviewImages.ExecuteDeleteAsync();
        await _context.Reviews.ExecuteDeleteAsync();
        await _context.OrderItems.ExecuteDeleteAsync();
        await _context.Orders.ExecuteDeleteAsync();
        await _context.CartItems.ExecuteDeleteAsync();
        await _context.Carts.ExecuteDeleteAsync();
        await _context.ProductTags.ExecuteDeleteAsync();
        await _context.Products.ExecuteDeleteAsync();
        await _context.Categories.ExecuteDeleteAsync();
        await _context.Vendors.ExecuteDeleteAsync();
        await _context.Users.ExecuteDeleteAsync();

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Resets the database completely (deletes and recreates schema).
    /// Use this only when you need a completely fresh database state.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        if (_context == null) return;

        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// Synchronously clears all entities from the database.
    /// </summary>
    public void ClearDatabase()
    {
        if (_context == null) return;

        // Delete data in order of foreign key dependencies
        _context.ReviewImages.ExecuteDelete();
        _context.Reviews.ExecuteDelete();
        _context.OrderItems.ExecuteDelete();
        _context.Orders.ExecuteDelete();
        _context.CartItems.ExecuteDelete();
        _context.Carts.ExecuteDelete();
        _context.ProductTags.ExecuteDelete();
        _context.Products.ExecuteDelete();
        _context.Categories.ExecuteDelete();
        _context.Vendors.ExecuteDelete();
        _context.Users.ExecuteDelete();

        _context.SaveChanges();
    }

    /// <summary>
    /// Gets the current database provider name (e.g., "sqlite", "sqlserver").
    /// </summary>
    public string GetDatabaseProvider()
        => _context?.Database.ProviderName ?? "unknown";

    /// <summary>
    /// Cleans up the database context and deletes the test database.
    /// </summary>
    public void Dispose()
    {
        if (_context != null)
        {
            try
            {
                if (!_isInMemory)
                {
                    _context.Database.EnsureDeleted();
                }
            }
            finally
            {
                _context.Dispose();
                _context = null;
            }
        }
    }

    /// <summary>
    /// Async cleanup of the database context.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            try
            {
                if (!_isInMemory)
                {
                    await _context.Database.EnsureDeletedAsync();
                }
            }
            finally
            {
                await _context.DisposeAsync();
                _context = null;
            }
        }
    }
}
