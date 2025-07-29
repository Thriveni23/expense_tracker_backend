using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class SourceRepository : GenericRepository<Source>, ISourceRepository
{
	public SourceRepository(ExpenseTrackerDBContext context) : base(context)
	{
	}

	// Implement custom methods if defined
}
