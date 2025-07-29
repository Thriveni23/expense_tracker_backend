using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Repositories;

public class SavingGoalsRepository : GenericRepository<SavingGoals>, ISavingGoalsRepository
{
    public SavingGoalsRepository(ExpenseTrackerDBContext context) : base(context)
    {
    }

    // Implement custom methods here if defined
}
