using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTrackerCrudWebAPI.Database
{
    public class ExpenseTrackerDBContext: IdentityDbContext<User> { 
        public ExpenseTrackerDBContext(DbContextOptions<ExpenseTrackerDBContext> options) : base(options)
        {
        }


       
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<SavingGoals> SavingGoals { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Source> Sources { get; set; }

    }
}
