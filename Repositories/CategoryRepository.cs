using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;


public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ExpenseTrackerDBContext context) : base(context)
    {
    }

    // Implement custom methods if defined
}
