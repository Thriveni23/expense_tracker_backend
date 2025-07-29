using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Models;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                CategoryType = c.CategoryType
            }).ToList();
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto)
        {
            var category = new Category
            {
                CategoryType = categoryDto.CategoryType
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            categoryDto.Id = category.Id;
            return categoryDto;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return false;

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
