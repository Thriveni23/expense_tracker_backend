using ExpenseTrackerCrudWebAPI.DTOs;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface ISavingGoalsService
    {
        Task<SavingGoalDTO> CreateGoalAsync(string userId, SavingGoalDTO goalDto);
        Task<List<SavingGoalDTO>> GetAllGoalsAsync(string userId);
        Task<SavingGoalDTO?> GetGoalByIdAsync(int id);
        Task<SavingGoalDTO?> UpdateGoalAsync(string userId, int id, SavingGoalDTO updatedGoalDto);
        Task<SavingGoalDTO?> AddToSavingsAsync(string userId, int id, decimal amount);
        Task<bool> DeleteGoalAsync(string userId, int id);
    }
}
