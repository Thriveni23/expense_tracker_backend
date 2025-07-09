using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(ExpenseTrackerDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        [HttpGet("all-incomes")]
        public async Task<IActionResult> GetAllIncomes()
        {
            var incomes = await _context.Incomes
                .Include(i => i.User)
                .Select(i => new
                {
                    i.Id,
                    i.Source,
                    i.Amount,
                    i.Date,
                    UserEmail = i.User.Email,
                    UserFullName = i.User.FirstName + " " + i.User.LastName
                })
                .ToListAsync();

            return Ok(incomes);
        }

        [HttpGet("all-savings")]
        public async Task<IActionResult> GetAllSavings()
        {
            var savings = await _context.SavingGoals
                .Include(s => s.User)
                .Select(s => new
                {
                    s.Id,
                    s.GoalName,
                    s.TargetAmount,
                    s.SavedAmount,
                    UserEmail = s.User.Email,
                    UserFullName = s.User.FirstName + " " + s.User.LastName
                })
                .ToListAsync();

            return Ok(savings);
        }


        [HttpGet("all-budgets")]
        public async Task<IActionResult> GetAllBudgets()
        {
            var budgets = await _context.Budgets
                .Include(b => b.User)
                .Select(b => new
                {
                    b.Id,
                    b.Category,
                    b.Amount,
                    b.MonthYear,
                    UserEmail = b.User.Email,
                    UserFullName = b.User.FirstName + " " + b.User.LastName
                })
                .ToListAsync();

            return Ok(budgets);
        }

        [HttpGet("all-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _context.Transactions
                .Include(t => t.User)
                .Select(t => new
                {
                    t.Id,
                 
                    t.Amount,
                    t.Category,
                    t.Description,
                    t.Date,
                    UserEmail = t.User.Email,
                    UserFullName = t.User.FirstName + " " + t.User.LastName
                })
                .ToListAsync();

            return Ok(transactions);
        }


        [HttpGet("all-users")]
    
        public async Task<IActionResult> GetAllUsers()
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

            return Ok(userDtos);
        }


        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var incomes = _context.Incomes.Where(i => i.UserId == id);
            _context.Incomes.RemoveRange(incomes);

            var expenses = _context.Transactions.Where(e => e.UserId == id);
            _context.Transactions.RemoveRange(expenses);

            var budgets = _context.Budgets.Where(e => e.UserId == id);
            _context.Budgets.RemoveRange(budgets);

            var savingGoals = _context.SavingGoals.Where(e => e.UserId == id);
            _context.SavingGoals.RemoveRange(savingGoals);


            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            return Ok(new { message = "User deleted successfully" });
        }



    }
}
