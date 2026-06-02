using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling user business logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");
            return await _unitOfWork.Users.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<User?> GetUserByIdAsync(string id)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user with email: {Email}", email);
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            _logger.LogInformation("Fetching all active users");
            return await _unitOfWork.Users.GetActiveUsersAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            _logger.LogInformation("Fetching users by role: {Role}", role);
            return await _unitOfWork.Users.GetByRoleAsync(role);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetVendorsAsync()
        {
            _logger.LogInformation("Fetching all vendors");
            return await _unitOfWork.Users.GetVendorsAsync();
        }

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            _logger.LogInformation("Creating new user: {Email}", user.Email);

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("User email cannot be empty", nameof(user.Email));

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("User password hash cannot be empty", nameof(user.PasswordHash));

            // Check if email already exists
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email already exists: {Email}", user.Email);
                throw new InvalidOperationException($"User with email '{user.Email}' already exists");
            }

            await _unitOfWork.Users.CreateAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
            return user;
        }

        /// <inheritdoc/>
        public async Task<User> UpdateUserAsync(string id, User user)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found for update: {UserId}", id);
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            // Check if email changed and if new email is unique
            if (existingUser.Email != user.Email)
            {
                var userWithEmail = await _unitOfWork.Users.GetByEmailAsync(user.Email);
                if (userWithEmail != null && userWithEmail.Id != id)
                {
                    _logger.LogWarning("User with email already exists: {Email}", user.Email);
                    throw new InvalidOperationException($"User with email '{user.Email}' already exists");
                }
            }

            user.Id = id;
            await _unitOfWork.Users.UpdateAsync(id, user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User updated successfully: {UserId}", id);
            return user;
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(string id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion: {UserId}", id);
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User deleted successfully: {UserId}", id);
        }

        /// <inheritdoc/>
        public async Task<User> VerifyEmailAsync(string id)
        {
            _logger.LogInformation("Verifying email for user: {UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", id);
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            user.IsEmailVerified = true;
            await _unitOfWork.Users.UpdateAsync(id, user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Email verified for user: {UserId}", id);
            return user;
        }

        /// <inheritdoc/>
        public async Task<User> SetUserActiveStatusAsync(string id, bool isActive)
        {
            _logger.LogInformation("Setting user active status - ID: {UserId}, IsActive: {IsActive}", id, isActive);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", id);
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            user.IsActive = isActive;
            await _unitOfWork.Users.UpdateAsync(id, user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User active status updated: {UserId}", id);
            return user;
        }
    }
}
