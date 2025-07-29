using ExpenseTrackerCrudWebAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface IAdminService
    {
        Task<List<IncomeDTO>> GetAllIncomesAsync();
        Task<List<SavingGoalDTO>> GetAllSavingsAsync();
        Task<List<BudgetDTO>> GetAllBudgetsAsync();
        Task<List<TransactionDTO>> GetAllTransactionsAsync();
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<bool> DeleteUserByIdAsync(string userId);
    }
}
