using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for vendor-specific operations
    /// </summary>
    public interface IVendorRepository : IRepository<Vendor>
    {
        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        Task<Vendor?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Get approved vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetApprovedVendorsAsync();

        /// <summary>
        /// Get active vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetActiveVendorsAsync();

        /// <summary>
        /// Get pending vendors (not approved)
        /// </summary>
        Task<IEnumerable<Vendor>> GetPendingVendorsAsync();

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10);
    }
}
