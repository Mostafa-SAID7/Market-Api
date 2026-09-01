using MediatR;
using Market.Domain.Enums;
using Market.Application.Features.Users.Commands;
using Market.Application.Features.Users.Queries;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var user = await _mediator.Send(query);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var query = new GetUserByEmailQuery { Email = email };
            var user = await _mediator.Send(query);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Get active users
        /// </summary>
        [HttpGet("active/list")]
        public async Task<IActionResult> GetActive()
        {
            var query = new GetActiveUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var query = new GetUsersByRoleQuery { Role = role };
            var users = await _mediator.Send(query);
            return Ok(users);
        }

        /// <summary>
        /// Get vendors
        /// </summary>
        [HttpGet("vendors/list")]
        public async Task<IActionResult> GetVendors()
        {
            var query = new GetVendorsQuery();
            var vendors = await _mediator.Send(query);
            return Ok(vendors);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _mediator.Send(command);
                return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid user data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Id = id;

            try
            {
                var updatedUser = await _mediator.Send(command);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteUserCommand { Id = id };
                await _mediator.Send(command);
                return Ok(new { success = true, message = "User deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verify user email
        /// </summary>
        [HttpPut("{id}/verify-email")]
        public async Task<IActionResult> VerifyEmail(int id)
        {
            try
            {
                var command = new VerifyEmailCommand { Id = id };
                var user = await _mediator.Send(command);
                return Ok(new { success = true, user });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Set user active status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetActiveStatus(int id, [FromBody] SetUserActiveStatusCommand command)
        {
            try
            {
                command.Id = id;
                var user = await _mediator.Send(command);
                return Ok(new { success = true, user });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

