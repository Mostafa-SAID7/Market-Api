using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly ILogger<VendorsController> _logger;

        public VendorsController(IVendorService vendorService, ILogger<VendorsController> logger)
        {
            _vendorService = vendorService;
            _logger = logger;
        }

        /// <summary>
        /// Get all vendors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        /// <summary>
        /// Get vendor by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }

        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var vendor = await _vendorService.GetVendorByUserIdAsync(userId);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }

        /// <summary>
        /// Get approved vendors
        /// </summary>
        [HttpGet("approved/list")]
        public async Task<IActionResult> GetApproved()
        {
            var vendors = await _vendorService.GetApprovedVendorsAsync();
            return Ok(vendors);
        }

        /// <summary>
        /// Get active vendors
        /// </summary>
        [HttpGet("active/list")]
        public async Task<IActionResult> GetActive()
        {
            var vendors = await _vendorService.GetActiveVendorsAsync();
            return Ok(vendors);
        }

        /// <summary>
        /// Get pending vendors
        /// </summary>
        [HttpGet("pending/list")]
        public async Task<IActionResult> GetPending()
        {
            var vendors = await _vendorService.GetPendingVendorsAsync();
            return Ok(vendors);
        }

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        [HttpGet("toprated/list")]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 10)
        {
            var vendors = await _vendorService.GetTopRatedVendorsAsync(count);
            return Ok(vendors);
        }

        /// <summary>
        /// Create a new vendor
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdVendor = await _vendorService.CreateVendorAsync(vendor);
                return CreatedAtAction(nameof(Get), new { id = createdVendor.Id }, createdVendor);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid vendor data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Resource not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing vendor
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedVendor = await _vendorService.UpdateVendorAsync(id, vendor);
                return Ok(updatedVendor);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a vendor
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _vendorService.DeleteVendorAsync(id);
                return Ok(new { success = true, message = "Vendor deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Approve a vendor
        /// </summary>
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(string id)
        {
            try
            {
                var vendor = await _vendorService.ApproveVendorAsync(id);
                return Ok(new { success = true, vendor });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Reject a vendor
        /// </summary>
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(string id)
        {
            try
            {
                var vendor = await _vendorService.RejectVendorAsync(id);
                return Ok(new { success = true, vendor });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Set vendor active status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetActiveStatus(string id, [FromBody] dynamic request)
        {
            try
            {
                bool isActive = request.isActive;
                var vendor = await _vendorService.SetVendorActiveStatusAsync(id, isActive);
                return Ok(new { success = true, vendor });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update vendor rating
        /// </summary>
        [HttpPut("{id}/rating")]
        public async Task<IActionResult> UpdateRating(string id, [FromBody] dynamic request)
        {
            try
            {
                double rating = request.rating;
                int reviewCount = request.reviewCount;
                var vendor = await _vendorService.UpdateVendorRatingAsync(id, rating, reviewCount);
                return Ok(new { success = true, vendor });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Vendor not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
