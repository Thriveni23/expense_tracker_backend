using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetDTO> CreateBudgetAsync(string userId, BudgetDTO budgetDto);
        Task<List<BudgetDTO>> GetAllBudgetsAsync(string userId);
        Task<List<BudgetSummary>> GetCurrentMonthBudgetSummaryAsync(string userId);
    }
}
