// Services/Interfaces/ICategoryService.cs
using ExpenseTrackerCrudWebAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategoriesAsync();
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}

