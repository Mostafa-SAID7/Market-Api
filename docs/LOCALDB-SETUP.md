# SQL Server LocalDB Setup Guide

This guide explains how to set up SQL Server LocalDB on Windows for local testing and development of the Market API.

## Overview

**SQL Server LocalDB** is a lightweight version of SQL Server Express designed for development and testing. It runs in a special execution mode that allows you to work with SQL Server databases without installing a full server instance.

- **Why LocalDB?** Lightweight, no installation hassle, per-user databases, fast startup
- **When to use?** Integration tests, local development, database schema validation
- **File-based?** Yes - databases are stored in `%LocalAppData%\Microsoft\Microsoft SQL Server Local DB\`

---

## Prerequisites

- **Windows OS** (Windows 10, 11, Windows Server 2016+)
- **Visual Studio 2017+** OR **.NET SDK 9.0+**
- **Administrator access** for initial setup

> LocalDB is **not available on macOS or Linux**. Use WSL2 or Docker on those platforms (see alternatives below).

---

## Installation

### Option 1: Via Visual Studio (Recommended)

If you have Visual Studio 2017 or later installed, LocalDB is likely already available.

**Verify installation:**
```powershell
sqllocaldb info
```

You should see output like:
```
MSSQLLocalDB
v17.0
```

### Option 2: Via .NET SDK

.NET 9.0 SDK includes LocalDB support:

```powershell
# Verify .NET SDK is installed
dotnet --version

# LocalDB should be available automatically
sqllocaldb info
```

### Option 3: Manual Installation

Download and install SQL Server Express with LocalDB:

1. Visit https://www.microsoft.com/en-us/sql-server/sql-server-editions-express
2. Download **SQL Server Express with Advanced Services**
3. Run the installer and select **LocalDB** in the feature selection
4. Complete the installation

---

## First-Time Setup

### 1. Create a LocalDB Instance

```powershell
sqllocaldb create "MarketApiDev"
sqllocaldb start "MarketApiDev"
```

### 2. Verify Connection

```powershell
sqlcmd -S "(localdb)\MarketApiDev" -Q "SELECT 1 AS Connection"
```

Expected output:
```
Connection
-----------
          1
```

### 3. List All Instances

```powershell
sqllocaldb info
```

---

## Connection Strings

Use these connection strings in your test fixtures or configuration:

### Standard (Integrated Authentication)
```
Server=(localdb)\MarketApiDev;Database=MarketDb;Trusted_Connection=True;
```

### With Specific Database
```
Server=(localdb)\MarketApiDev;Database=MarketDb_{TestName};Trusted_Connection=True;
```

### In Code (DbContextFixture)
```csharp
var fixture = DbContextFixture.CreateLocalDb("MarketDb_IntegrationTests");
var context = fixture.GetContext();
```

---

## Daily Workflow

### Start LocalDB (if not running)
```powershell
sqllocaldb start "MarketApiDev"
```

### Run Tests
```powershell
dotnet test --configuration Release
```

### Stop LocalDB (cleanup)
```powershell
sqllocaldb stop "MarketApiDev"
```

### View All Instances
```powershell
sqllocaldb info
```

---

## Troubleshooting

### Issue: "sqllocaldb command not found"

**Cause:** LocalDB is not installed or not in PATH.

**Solution:**
```powershell
# Reinstall LocalDB via Visual Studio Installer or SQL Server Express
# Or manually add to PATH: C:\Program Files\Microsoft SQL Server\150\Tools\Binn\
```

### Issue: "Application Control policy has blocked this file"

**Cause:** Windows AppControl prevents DLL execution (affects test runners).

**Solution:** This blocks local testing but not CI/CD. Use one of:
1. **Disable AppControl** (requires admin):
   ```powershell
   # In PowerShell as Administrator:
   Set-AppXPackage -AllUsers -Name Microsoft.Windows.AppNotifications | Remove-AppxPackage
   ```
2. **Use WSL2** (recommended alternative)
3. **Use Docker** (recommended alternative)

### Issue: "Cannot attach database" or "Permission denied"

**Cause:** Database file permissions or LocalDB service issue.

**Solution:**
```powershell
# Restart LocalDB instance
sqllocaldb stop "MarketApiDev"
sqllocaldb start "MarketApiDev"

# Or delete and recreate
sqllocaldb delete "MarketApiDev"
sqllocaldb create "MarketApiDev"
sqllocaldb start "MarketApiDev"
```

### Issue: "Database already exists" during migration

**Cause:** Test database was not cleaned up after previous test run.

**Solution:**
```powershell
# Connect and drop the database:
sqlcmd -S "(localdb)\MarketApiDev" -Q "DROP DATABASE MarketDb_Tests"

# Or in test code use DbContextFixture.ResetDatabaseAsync()
```

### Issue: Out of disk space

**Cause:** Old test databases accumulating in `%LocalAppData%`.

**Solution:**
```powershell
# List all LocalDB databases
sqlcmd -S "(localdb)\MarketApiDev" -Q "SELECT name FROM sys.databases WHERE name LIKE 'MarketDb_%'"

# Delete old test databases
sqlcmd -S "(localdb)\MarketApiDev" -Q "DROP DATABASE MarketDb_OldTest"

# Or manually cleanup: %LocalAppData%\Microsoft\Microsoft SQL Server Local DB\
```

---

## Alternative: Using WSL2 (Linux on Windows)

If LocalDB doesn't work, use SQL Server in WSL2:

### 1. Install SQL Server in WSL2
```bash
# In WSL2 bash
curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
curl https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2019.list | sudo tee /etc/apt/sources.list.d/mssql-server-list.list
sudo apt update
sudo apt install mssql-server
/opt/mssql/bin/sqlservr
```

### 2. Connect from Windows
```powershell
# Get WSL2 IP address
wsl hostname -I

# Use in connection string
"Server=<WSL2_IP>,1433;Database=MarketDb;User Id=sa;Password=YourPassword;"
```

---

## Alternative: Using Docker

```powershell
# Pull and run SQL Server image
docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# Connection string
"Server=localhost,1433;Database=MarketDb;User Id=sa;Password=YourPassword123!;"
```

---

## Integration Test Configuration

### Using LocalDB Fixture in Tests

```csharp
public class ProductRepositoryTests : IDisposable
{
    private readonly DbContextFixture _fixture;

    public ProductRepositoryTests()
    {
        // Use LocalDB for integration tests
        _fixture = DbContextFixture.CreateLocalDb("MarketDb_ProductTests");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct()
    {
        var context = _fixture.GetContext();
        var product = ProductBuilder.Default().Build();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var result = await repo.GetByIdAsync(product.Id, CancellationToken.None);

        Assert.NotNull(result);
    }

    public void Dispose() => _fixture.Dispose();
}
```

### Using In-Memory SQLite (Faster, No Setup Required)

```csharp
// For unit tests, prefer in-memory SQLite (no LocalDB needed)
public class CartTests
{
    [Fact]
    public void AddItem_MergesProducts()
    {
        var fixture = DbContextFixture.CreateInMemory(); // SQLite, not LocalDB
        var context = fixture.GetContext();
        
        // ... test code ...
        
        fixture.Dispose();
    }
}
```

---

## Best Practices

1. **Use in-memory SQLite for unit tests** - Fast, no setup required
2. **Use LocalDB for integration tests** - Tests real DB schema and constraints
3. **Clear database between tests** - Call `DbFixture.ClearDatabaseAsync()`
4. **Use builders for test data** - Consistent, readable test setup
5. **Stop LocalDB when done** - Saves system resources
6. **Keep connection strings in config** - Don't hardcode instances

---

## Quick Reference

```powershell
# Create instance
sqllocaldb create "MarketApiDev"

# Start instance
sqllocaldb start "MarketApiDev"

# Stop instance
sqllocaldb stop "MarketApiDev"

# Delete instance
sqllocaldb delete "MarketApiDev"

# List instances
sqllocaldb info

# Connect via SSMS or sqlcmd
sqlcmd -S "(localdb)\MarketApiDev"

# Run tests with coverage
dotnet test --configuration Release --collect:"XPlat Code Coverage"
```

---

## Support

For issues or questions:
1. Check [Troubleshooting](#troubleshooting) section above
2. Review [Test Documentation](./TESTING.md)
3. Consult [Microsoft LocalDB Documentation](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
