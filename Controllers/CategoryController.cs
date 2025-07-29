// Controllers/CategoryController.cs
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            _logger.LogInformation("Fetching all categories");
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching categories");
                return StatusCode(500, "An error occurred while retrieving categories.");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO categoryDto)
        {
            _logger.LogInformation("Adding new category: {CategoryType}", categoryDto.CategoryType);
            try
            {
                var added = await _categoryService.AddCategoryAsync(categoryDto);
                return Ok(added);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category: {CategoryType}", categoryDto.CategoryType);
                return StatusCode(500, "An error occurred while adding the category.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
            try
            {
                var deleted = await _categoryService.DeleteCategoryAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {CategoryId}", id);
                return StatusCode(500, "An error occurred while deleting the category.");
            }
        }
    }
}