using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
	public TransactionRepository(ExpenseTrackerDBContext context) : base(context)
	{
	}
    public IQueryable<Transaction> GetQueryable()
    {
        return _context.Transactions.AsQueryable();
    }

   
}
