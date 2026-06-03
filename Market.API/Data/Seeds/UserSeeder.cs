using Market.API.Models.Entities;
using Market.API.Models.Enums;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds users (admin, vendors, customers)
    /// </summary>
    public class UserSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<UserSeeder> _logger;

        public UserSeeder(MongoDbContext context, ILogger<UserSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                var existingUsers = await _context.Users.CountDocumentsAsync(FilterDefinition<User>.Empty);
                if (existingUsers > 0)
                {
                    _logger.LogInformation("Users already exist. Skipping seeding.");
                    return;
                }

                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin@market.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        FirstName = "Admin",
                        LastName = "User",
                        PhoneNumber = "+1234567890",
                        Role = UserRole.Admin,
                        IsActive = true,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Email = "vendor1@market.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Vendor@123"),
                        FirstName = "John",
                        LastName = "Vendor",
                        PhoneNumber = "+0987654321",
                        Role = UserRole.Vendor,
                        IsActive = true,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Email = "customer1@market.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                        FirstName = "Jane",
                        LastName = "Customer",
                        PhoneNumber = "+1122334455",
                        Role = UserRole.Customer,
                        IsActive = true,
                        IsEmailVerified = true
                    }
                };

                await _context.Users.InsertManyAsync(users);
                _logger.LogInformation($"Seeded {users.Count} users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding users");
                throw;
            }
        }
    }
}
