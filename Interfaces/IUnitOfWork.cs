using System;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Models;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }

        ITransactionRepository Transactions { get; }
        IIncomeRepository Incomes { get; }
        ISavingGoalsRepository SavingGoals { get; }
        ISourceRepository Sources { get; }
        ICategoryRepository Categories { get; }
        IBudgetRepository Budgets { get; }

        Task<int> CompleteAsync();
    }
}
