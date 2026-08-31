using MediatR;
using Market.API.Features.Vendors.Commands;
using Market.API.Features.Vendors.Queries;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VendorsController> _logger;

        public VendorsController(IMediator mediator, ILogger<VendorsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all vendors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllVendorsQuery();
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Get vendor by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetVendorByIdQuery { Id = id };
            var vendor = await _mediator.Send(query);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }

        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var query = new GetVendorByUserIdQuery { UserId = userId };
            var vendor = await _mediator.Send(query);
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
            var query = new GetApprovedVendorsQuery();
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Get active vendors
        /// </summary>
        [HttpGet("active/list")]
        public async Task<IActionResult> GetActive()
        {
            var query = new GetActiveVendorsQuery();
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Get pending vendors
        /// </summary>
        [HttpGet("pending/list")]
        public async Task<IActionResult> GetPending()
        {
            var query = new GetPendingVendorsQuery();
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        [HttpGet("toprated/list")]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 10)
        {
            var query = new GetTopRatedVendorsQuery { Count = count };
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Create a new vendor
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVendorCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdVendor = await _mediator.Send(command);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVendorCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Id = id;

            try
            {
                var updatedVendor = await _mediator.Send(command);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteVendorCommand { Id = id };
                await _mediator.Send(command);
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
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var command = new ApproveVendorCommand { Id = id };
                var vendor = await _mediator.Send(command);
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
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var command = new RejectVendorCommand { Id = id };
                var vendor = await _mediator.Send(command);
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
        public async Task<IActionResult> SetActiveStatus(int id, [FromBody] SetVendorActiveStatusCommand command)
        {
            try
            {
                command.Id = id;
                var vendor = await _mediator.Send(command);
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
        public async Task<IActionResult> UpdateRating(int id, [FromBody] UpdateVendorRatingCommand command)
        {
            try
            {
                command.Id = id;
                var vendor = await _mediator.Send(command);
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
