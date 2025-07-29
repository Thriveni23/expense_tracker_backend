using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
	public TransactionRepository(ExpenseTrackerDBContext context) : base(context)
	{
	}

	// Implement custom methods here if defined
}
