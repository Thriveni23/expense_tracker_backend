using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Services;
using ExpenseTrackerCrudWebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("all-incomes")]
        public async Task<IActionResult> GetAllIncomes()
        {
            try
            {
                var incomes = await _adminService.GetAllIncomesAsync();
                return Ok(incomes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all incomes");
                return StatusCode(500, "Error fetching incomes");
            }
        }

        [HttpGet("all-savings")]
        public async Task<IActionResult> GetAllSavings()
        {
            try
            {
                var savings = await _adminService.GetAllSavingsAsync();
                return Ok(savings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all savings");
                return StatusCode(500, "Error fetching savings");
            }
        }

        [HttpGet("all-budgets")]
        public async Task<IActionResult> GetAllBudgets()
        {
            try
            {
                var budgets = await _adminService.GetAllBudgetsAsync();
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all budgets");
                return StatusCode(500, "Error fetching budgets");
            }
        }

        [HttpGet("all-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _adminService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all transactions");
                return StatusCode(500, "Error fetching transactions");
            }
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return StatusCode(500, "Error fetching users");
            }
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                var deleted = await _adminService.DeleteUserByIdAsync(id);
                if (!deleted) return NotFound(new { message = "User not found" });

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, "Error deleting user");
            }
        }
    }
}