using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class IncomeRepository : GenericRepository<Income>, IIncomeRepository
{
    public IncomeRepository(ExpenseTrackerDBContext context) : base(context)
    {
    }

    // Implement custom methods if defined
}
