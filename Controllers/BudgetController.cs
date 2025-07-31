using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
   
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly ILogger<BudgetController> _logger;

        public BudgetController(IBudgetService budgetService, ILogger<BudgetController> logger)
        {
            _budgetService = budgetService;
            _logger = logger;
        }

        private string? GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateBudget([FromBody] BudgetDTO budgetDto)
        {
            var userId = GetUserId();
            _logger.LogInformation("User {UserId} is creating a budget", userId);

            try
            {
                var createdBudget = await _budgetService.CreateBudgetAsync(userId, budgetDto);
                _logger.LogInformation("Budget created successfully for user {UserId}", userId);
                return Ok(createdBudget);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating budget for user {UserId}", userId);
                return StatusCode(500, "An error occurred while creating the budget.");
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllBudgets()
        {
            var userId = GetUserId();
            _logger.LogInformation("Fetching all budgets for user {UserId}", userId);
            try
            {
                var budgets = await _budgetService.GetAllBudgetsAsync(userId);
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching budgets for user {UserId}", userId);
                return StatusCode(500, "An error occurred while fetching budgets.");
            }
        }

        [HttpGet("current-summary")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCurrentMonthBudgetSummary()
        {
            var userId = GetUserId();
            _logger.LogInformation("Fetching current month budget summary for user {UserId}", userId);
            try
            {
                var summary = await _budgetService.GetCurrentMonthBudgetSummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching current month summary for user {UserId}", userId);
                return StatusCode(500, "An error occurred while fetching the summary.");
            }
        }
    }
}