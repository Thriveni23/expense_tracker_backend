using ExpenseTrackerCrudWebAPI.DTOs;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface IIncomeService
    {
        Task<IncomeDTO> CreateIncomeAsync(IncomeDTO incomeDto, string userId);
        Task<IEnumerable<IncomeDTO>> GetAllIncomesAsync(string userId);
        Task<IncomeDTO?> GetIncomeByIdAsync(int id);
        Task<IncomeDTO?> UpdateIncomeAsync(int id, IncomeDTO updatedIncomeDto, string userId);
        Task<bool> DeleteIncomeAsync(int id, string userId);
    }
}
