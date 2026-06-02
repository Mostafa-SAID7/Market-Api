using Market.API.Models.Entities;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for vendor operations
    /// </summary>
    public interface IVendorService
    {
        /// <summary>
        /// Get all vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();

        /// <summary>
        /// Get vendor by ID
        /// </summary>
        Task<Vendor?> GetVendorByIdAsync(string id);

        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        Task<Vendor?> GetVendorByUserIdAsync(string userId);

        /// <summary>
        /// Get approved vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetApprovedVendorsAsync();

        /// <summary>
        /// Get active vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetActiveVendorsAsync();

        /// <summary>
        /// Get pending vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetPendingVendorsAsync();

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10);

        /// <summary>
        /// Create a new vendor
        /// </summary>
        Task<Vendor> CreateVendorAsync(Vendor vendor);

        /// <summary>
        /// Update an existing vendor
        /// </summary>
        Task<Vendor> UpdateVendorAsync(string id, Vendor vendor);

        /// <summary>
        /// Delete a vendor
        /// </summary>
        Task DeleteVendorAsync(string id);

        /// <summary>
        /// Approve a vendor
        /// </summary>
        Task<Vendor> ApproveVendorAsync(string id);

        /// <summary>
        /// Reject a vendor
        /// </summary>
        Task<Vendor> RejectVendorAsync(string id);

        /// <summary>
        /// Set vendor active status
        /// </summary>
        Task<Vendor> SetVendorActiveStatusAsync(string id, bool isActive);

        /// <summary>
        /// Update vendor rating
        /// </summary>
        Task<Vendor> UpdateVendorRatingAsync(string id, double rating, int reviewCount);
    }
}
