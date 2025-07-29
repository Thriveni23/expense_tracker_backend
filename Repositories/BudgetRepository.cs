using ExpenseTrackerCrudWebAPI.Models;

using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
	public BudgetRepository(ExpenseTrackerDBContext context) : base(context)
	{
	}

	// Implement custom methods if defined
}
