using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class SavingGoalsService : ISavingGoalsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SavingGoalsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SavingGoalDTO> CreateGoalAsync(string userId, SavingGoalDTO goalDto)
        {
            var goal = new SavingGoals
            {
                GoalName = goalDto.GoalName,
                TargetAmount = goalDto.TargetAmount,
                SavedAmount = goalDto.SavedAmount,
                UserId = userId
            };

            await _unitOfWork.SavingGoals.AddAsync(goal);
            await _unitOfWork.CompleteAsync();

            return new SavingGoalDTO
            {
                Id = goal.Id,
                GoalName = goal.GoalName,
                TargetAmount = goal.TargetAmount,
                SavedAmount = goal.SavedAmount,
                UserId = goal.UserId
            };
        }

        public async Task<List<SavingGoalDTO>> GetAllGoalsAsync(string userId)
        {
            var goals = await _unitOfWork.SavingGoals.GetAllAsync();
            return goals.Where(g => g.UserId == userId)
                        .Select(g => new SavingGoalDTO
                        {
                            Id = g.Id,
                            GoalName = g.GoalName,
                            TargetAmount = g.TargetAmount,
                            SavedAmount = g.SavedAmount,
                            UserId = g.UserId
                        }).ToList();
        }

        public async Task<SavingGoalDTO?> GetGoalByIdAsync(int id)
        {
            var goal = await _unitOfWork.SavingGoals.GetByIdAsync(id);
            if (goal == null) return null;

            return new SavingGoalDTO
            {
                Id = goal.Id,
                GoalName = goal.GoalName,
                TargetAmount = goal.TargetAmount,
                SavedAmount = goal.SavedAmount,
                UserId = goal.UserId
            };
        }

        public async Task<SavingGoalDTO?> UpdateGoalAsync(string userId, int id, SavingGoalDTO updatedDto)
        {
            var existing = await _unitOfWork.SavingGoals.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return null;

            existing.GoalName = updatedDto.GoalName;
            existing.TargetAmount = updatedDto.TargetAmount;
            existing.SavedAmount = updatedDto.SavedAmount;

            _unitOfWork.SavingGoals.Update(existing);
            await _unitOfWork.CompleteAsync();

            return new SavingGoalDTO
            {
                Id = existing.Id,
                GoalName = existing.GoalName,
                TargetAmount = existing.TargetAmount,
                SavedAmount = existing.SavedAmount,
                UserId = existing.UserId
            };
        }

        public async Task<SavingGoalDTO?> AddToSavingsAsync(string userId, int id, decimal amount)
        {
            var existing = await _unitOfWork.SavingGoals.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return null;

            existing.SavedAmount += amount;

            _unitOfWork.SavingGoals.Update(existing);
            await _unitOfWork.CompleteAsync();

            return new SavingGoalDTO
            {
                Id = existing.Id,
                GoalName = existing.GoalName,
                TargetAmount = existing.TargetAmount,
                SavedAmount = existing.SavedAmount,
                UserId = existing.UserId
            };
        }

        public async Task<bool> DeleteGoalAsync(string userId, int id)
        {
            var goal = await _unitOfWork.SavingGoals.GetByIdAsync(id);
            if (goal == null || goal.UserId != userId)
                return false;

            _unitOfWork.SavingGoals.Delete(goal);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
