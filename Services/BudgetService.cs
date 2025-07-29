using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BudgetDTO> CreateBudgetAsync(string userId, BudgetDTO budgetDto)
        {
            var budget = new Budget
            {
                UserId = userId,
                Category = budgetDto.Category,
                Amount = budgetDto.Amount,
                MonthYear = DateTime.Now.ToString("yyyy-MM")
            };

            await _unitOfWork.Budgets.AddAsync(budget);
            await _unitOfWork.CompleteAsync();

            return new BudgetDTO
            {
                Id = budget.Id,
                Category = budget.Category,
                Amount = budget.Amount,
                MonthYear = budget.MonthYear,
                UserId = budget.UserId
            };
        }

        public async Task<List<BudgetDTO>> GetAllBudgetsAsync(string userId)
        {
            var allBudgets = await _unitOfWork.Budgets.GetAllAsync();
            return allBudgets.Where(b => b.UserId == userId)
                             .Select(b => new BudgetDTO
                             {
                                 Id = b.Id,
                                 Category = b.Category,
                                 Amount = b.Amount,
                                 MonthYear = b.MonthYear,
                                 UserId = b.UserId
                             }).ToList();
        }

        public async Task<List<BudgetSummary>> GetCurrentMonthBudgetSummaryAsync(string userId)
        {
            var now = DateTime.Now;
            string currentMonthYear = $"{now.Year}-{now.Month:D2}";

            var allBudgets = await _unitOfWork.Budgets.GetAllAsync();
            var budgets = allBudgets.Where(b => b.UserId == userId && b.MonthYear == currentMonthYear);

            var allTransactions = await _unitOfWork.Transactions.GetAllAsync();
            var expenses = allTransactions
                .Where(e => e.UserId == userId && e.Date.Year == now.Year && e.Date.Month == now.Month)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(e => e.Amount)
                })
                .ToList();

            var summary = from b in budgets
                          join e in expenses on b.Category equals e.Category into exp
                          from e in exp.DefaultIfEmpty()
                          select new BudgetSummary
                          {
                              Category = b.Category,
                              Amount = b.Amount,
                              Spent = e?.TotalSpent ?? 0,
                              Remaining = b.Amount - (e?.TotalSpent ?? 0)
                          };

            return summary.ToList();
        }
    }
}