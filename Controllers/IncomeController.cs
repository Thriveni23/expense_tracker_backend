using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Services;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly ILogger<IncomesController> _logger;

        public IncomesController(IIncomeService incomeService, ILogger<IncomesController> logger)
        {
            _incomeService = incomeService;
            _logger = logger;
        }

        private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromBody] IncomeDTO incomeDto)
        {
            var userId = GetUserId();
            _logger.LogInformation("Creating income for user {UserId}", userId);

            try
            {
                var result = await _incomeService.CreateIncomeAsync(incomeDto, userId!);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating income");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncomes()
        {
            var userId = GetUserId();
            var result = await _incomeService.GetAllIncomesAsync(userId!);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeById(int id)
        {
            var result = await _incomeService.GetIncomeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome(int id, [FromBody] IncomeDTO incomeDto)
        {
            var userId = GetUserId();
            var result = await _incomeService.UpdateIncomeAsync(id, incomeDto, userId!);
            if (result == null) return Forbid();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var userId = GetUserId();
            var success = await _incomeService.DeleteIncomeAsync(id, userId!);
            if (!success) return Forbid();
            return Ok(new { message = "Income deleted successfully" });
        }
    }
}