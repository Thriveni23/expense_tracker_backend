using ExpenseTrackerAPI.Models;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    IQueryable<Transaction> GetQueryable();
}
