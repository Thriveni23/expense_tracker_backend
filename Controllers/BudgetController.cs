using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Models;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;

        public BudgetController(ExpenseTrackerDBContext context)
        {
            _context = context;
        }


        [HttpPost] 
        [Authorize]
        public async Task<IActionResult> CreateBudget(Budget budget)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            budget.UserId = userId;

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return Ok(budget);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBudgets()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var budgets = await _context.Budgets
                                .Where(i => i.UserId == userId)
                                .ToListAsync();

            return Ok(budgets);
        }

        [HttpGet("current-summary")]
        [Authorize]
        public async Task<IActionResult> GetCurrentMonthBudgetSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var now = DateTime.Now;
            string currentMonthYear = $"{now.Year}-{now.Month:D2}";  

           
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId && b.MonthYear == currentMonthYear)
                .ToListAsync();

          
            var expenses = await _context.Transactions
                .Where(e => e.UserId == userId && e.Date.Year == now.Year && e.Date.Month == now.Month)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSpent = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            
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

            return Ok(summary);
        }
    }
}
