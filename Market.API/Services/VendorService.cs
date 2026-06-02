using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling vendor business logic
    /// </summary>
    public class VendorService : IVendorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VendorService> _logger;

        public VendorService(IUnitOfWork unitOfWork, ILogger<VendorService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            _logger.LogInformation("Fetching all vendors");
            return await _unitOfWork.Vendors.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Vendor?> GetVendorByIdAsync(string id)
        {
            _logger.LogInformation("Fetching vendor with ID: {VendorId}", id);
            return await _unitOfWork.Vendors.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Vendor?> GetVendorByUserIdAsync(string userId)
        {
            _logger.LogInformation("Fetching vendor for user: {UserId}", userId);
            return await _unitOfWork.Vendors.GetByUserIdAsync(userId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetApprovedVendorsAsync()
        {
            _logger.LogInformation("Fetching all approved vendors");
            return await _unitOfWork.Vendors.GetApprovedVendorsAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetActiveVendorsAsync()
        {
            _logger.LogInformation("Fetching all active vendors");
            return await _unitOfWork.Vendors.GetActiveVendorsAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetPendingVendorsAsync()
        {
            _logger.LogInformation("Fetching pending vendors");
            return await _unitOfWork.Vendors.GetPendingVendorsAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10)
        {
            _logger.LogInformation("Fetching top {Count} rated vendors", count);
            return await _unitOfWork.Vendors.GetTopRatedVendorsAsync(count);
        }

        /// <inheritdoc/>
        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {
            _logger.LogInformation("Creating new vendor: {StoreName}", vendor.StoreName);

            if (string.IsNullOrWhiteSpace(vendor.StoreName))
                throw new ArgumentException("Store name cannot be empty", nameof(vendor.StoreName));

            if (string.IsNullOrWhiteSpace(vendor.UserId))
                throw new ArgumentException("User ID cannot be empty", nameof(vendor.UserId));

            // Check if vendor already exists for user
            var existingVendor = await _unitOfWork.Vendors.GetByUserIdAsync(vendor.UserId);
            if (existingVendor != null)
            {
                _logger.LogWarning("Vendor already exists for user: {UserId}", vendor.UserId);
                throw new InvalidOperationException($"Vendor already exists for user {vendor.UserId}");
            }

            // Verify user exists
            var user = await _unitOfWork.Users.GetByIdAsync(vendor.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for vendor creation: {UserId}", vendor.UserId);
                throw new KeyNotFoundException($"User with ID {vendor.UserId} not found");
            }

            await _unitOfWork.Vendors.CreateAsync(vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor created successfully with ID: {VendorId}", vendor.Id);
            return vendor;
        }

        /// <inheritdoc/>
        public async Task<Vendor> UpdateVendorAsync(string id, Vendor vendor)
        {
            _logger.LogInformation("Updating vendor with ID: {VendorId}", id);

            var existingVendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (existingVendor == null)
            {
                _logger.LogWarning("Vendor not found for update: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            vendor.Id = id;
            vendor.UserId = existingVendor.UserId; // Prevent user ID change
            await _unitOfWork.Vendors.UpdateAsync(id, vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor updated successfully: {VendorId}", id);
            return vendor;
        }

        /// <inheritdoc/>
        public async Task DeleteVendorAsync(string id)
        {
            _logger.LogInformation("Deleting vendor with ID: {VendorId}", id);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found for deletion: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            // Check if vendor has products
            var allProducts = await _unitOfWork.Products.GetAllAsync();
            if (allProducts.Any(p => p.VendorId == id))
            {
                _logger.LogWarning("Cannot delete vendor with products: {VendorId}", id);
                throw new InvalidOperationException("Cannot delete vendor that has associated products");
            }

            await _unitOfWork.Vendors.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor deleted successfully: {VendorId}", id);
        }

        /// <inheritdoc/>
        public async Task<Vendor> ApproveVendorAsync(string id)
        {
            _logger.LogInformation("Approving vendor with ID: {VendorId}", id);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            vendor.IsApproved = true;
            await _unitOfWork.Vendors.UpdateAsync(id, vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor approved: {VendorId}", id);
            return vendor;
        }

        /// <inheritdoc/>
        public async Task<Vendor> RejectVendorAsync(string id)
        {
            _logger.LogInformation("Rejecting vendor with ID: {VendorId}", id);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            vendor.IsApproved = false;
            await _unitOfWork.Vendors.UpdateAsync(id, vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor rejected: {VendorId}", id);
            return vendor;
        }

        /// <inheritdoc/>
        public async Task<Vendor> SetVendorActiveStatusAsync(string id, bool isActive)
        {
            _logger.LogInformation("Setting vendor active status - ID: {VendorId}, IsActive: {IsActive}", id, isActive);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            vendor.IsActive = isActive;
            await _unitOfWork.Vendors.UpdateAsync(id, vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor active status updated: {VendorId}", id);
            return vendor;
        }

        /// <inheritdoc/>
        public async Task<Vendor> UpdateVendorRatingAsync(string id, double rating, int reviewCount)
        {
            _logger.LogInformation("Updating vendor rating - ID: {VendorId}, Rating: {Rating}, ReviewCount: {ReviewCount}", id, rating, reviewCount);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(id);
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found: {VendorId}", id);
                throw new KeyNotFoundException($"Vendor with ID {id} not found");
            }

            vendor.AverageRating = rating;
            vendor.TotalReviews = reviewCount;
            await _unitOfWork.Vendors.UpdateAsync(id, vendor);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Vendor rating updated: {VendorId}", id);
            return vendor;
        }
    }
}
