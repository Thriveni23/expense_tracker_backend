using System.Threading.Tasks;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Repositories;


namespace ExpenseTrackerCrudWebAPI.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExpenseTrackerDBContext _context;
        public IGenericRepository<User> Users { get;  set; }
        public ITransactionRepository Transactions { get; }
        public IIncomeRepository Incomes { get; }
        public ISavingGoalsRepository SavingGoals { get; }
        public ISourceRepository Sources { get; }
        public ICategoryRepository Categories { get; }
        public IBudgetRepository Budgets { get; }


        public UnitOfWork(ExpenseTrackerDBContext context)
        {
            _context = context;
            Users = new GenericRepository<User>(_context);

            Transactions = new TransactionRepository(_context);
            Incomes = new IncomeRepository(_context);
            SavingGoals = new SavingGoalsRepository(_context);
            Sources = new SourceRepository(_context);
            Categories = new CategoryRepository(_context);
            Budgets = new BudgetRepository(_context);

        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}