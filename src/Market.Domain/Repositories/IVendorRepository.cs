using Market.Domain.Entities;

namespace Market.Domain.Repositories
{
    /// <summary>
    /// Repository interface for vendor-specific operations
    /// </summary>
    public interface IVendorRepository : IRepository<Vendor>
    {
        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        Task<Vendor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get approved vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetApprovedVendorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get active vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get pending vendors (not approved)
        /// </summary>
        Task<IEnumerable<Vendor>> GetPendingVendorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10, CancellationToken cancellationToken = default);
    }
}


