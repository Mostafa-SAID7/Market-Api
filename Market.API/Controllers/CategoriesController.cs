using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        /// <summary>
        /// Get category by slug
        /// </summary>
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        /// <summary>
        /// Get active categories
        /// </summary>
        [HttpGet("active/list")]
        public async Task<IActionResult> GetActive()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get root categories
        /// </summary>
        [HttpGet("root/list")]
        public async Task<IActionResult> GetRootCategories()
        {
            var categories = await _categoryService.GetRootCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get subcategories by parent ID
        /// </summary>
        [HttpGet("{parentCategoryId}/subcategories")]
        public async Task<IActionResult> GetSubCategories(string parentCategoryId)
        {
            var categories = await _categoryService.GetSubCategoriesAsync(parentCategoryId);
            return Ok(categories);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
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
        public async Task<IActionResult> Update(string id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
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
        public async Task<IActionResult> SetActiveStatus(string id, [FromBody] dynamic request)
        {
            try
            {
                bool isActive = request.isActive;
                var category = await _categoryService.SetCategoryActiveStatusAsync(id, isActive);
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
