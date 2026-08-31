using MediatR;
using Market.API.Features.Categories.Commands;
using Market.API.Features.Categories.Queries;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var category = await _mediator.Send(query);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCategory = await _mediator.Send(command);
                return CreatedAtAction(nameof(Get), new { id = createdCategory.Id }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid category data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Id = id;

            try
            {
                var updatedCategory = await _mediator.Send(command);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Category not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteCategoryCommand { Id = id };
                await _mediator.Send(command);
                return Ok(new { success = true, message = "Category deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Category not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Set category active status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetActiveStatus(int id, [FromBody] SetCategoryActiveStatusCommand command)
        {
            try
            {
                command.Id = id;
                var category = await _mediator.Send(command);
                return Ok(new { success = true, category });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Category not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
