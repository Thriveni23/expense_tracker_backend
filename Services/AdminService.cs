using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<IncomeDTO>> GetAllIncomesAsync()
        {
            var incomes = await _unitOfWork.Incomes.GetAllAsync();
            return incomes.Select(i => new IncomeDTO
            {
                Id = i.Id,
                Amount = i.Amount,
                Date = i.Date,
                Source = i.Source,
                UserId = i.UserId
            }).ToList();
        }

        public async Task<List<SavingGoalDTO>> GetAllSavingsAsync()
        {
            var savings = await _unitOfWork.SavingGoals.GetAllAsync();
            return savings.Select(s => new SavingGoalDTO
            {
                Id = s.Id,
                GoalName = s.GoalName,
                TargetAmount = s.TargetAmount,
                SavedAmount = s.SavedAmount,
                UserId = s.UserId
            }).ToList();
        }

        public async Task<List<BudgetDTO>> GetAllBudgetsAsync()
        {
            var budgets = await _unitOfWork.Budgets.GetAllAsync();
            return budgets.Select(b => new BudgetDTO
            {
                Id = b.Id,
                Category = b.Category,
                MonthYear = b.MonthYear,
                Amount = b.Amount,
                UserId = b.UserId
            }).ToList();
        }

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            return transactions.Select(t => new TransactionDTO
            {
                Id = t.Id,
                Amount = t.Amount,
                Category = t.Category,
                Date = t.Date,
                Description = t.Description,
                UserId = t.UserId
            }).ToList();
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? "User"
                });
            }

            return userDtos;
        }

        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
