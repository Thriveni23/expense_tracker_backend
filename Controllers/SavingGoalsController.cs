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
    public class SavingGoalsController : ControllerBase
    {
        private readonly ISavingGoalsService _savingGoalsService;
        private readonly ILogger<SavingGoalsController> _logger;

        public SavingGoalsController(ISavingGoalsService savingGoalsService, ILogger<SavingGoalsController> logger)
        {
            _savingGoalsService = savingGoalsService;
            _logger = logger;
        }

        private string? GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateGoal([FromBody] SavingGoalDTO goalDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            _logger.LogInformation("Creating saving goal: {GoalName}", goalDto.GoalName);
            try
            {
                var created = await _savingGoalsService.CreateGoalAsync(userId, goalDto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating goal: {GoalName}", goalDto.GoalName);
                return StatusCode(500, "An error occurred while creating the saving goal.");
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllGoals()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            _logger.LogInformation("Fetching all saving goals for user {UserId}", userId);
            try
            {
                var goals = await _savingGoalsService.GetAllGoalsAsync(userId);
                return Ok(goals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching saving goals for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving goals.");
            }
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGoalById(int id)
        {
            _logger.LogInformation("Fetching saving goal with ID {GoalId}", id);
            try
            {
                var goal = await _savingGoalsService.GetGoalByIdAsync(id);
                if (goal == null)
                {
                    _logger.LogWarning("Saving goal with ID {GoalId} not found", id);
                    return NotFound();
                }
                return Ok(goal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching saving goal with ID {GoalId}", id);
                return StatusCode(500, "An error occurred while retrieving the goal.");
            }
        }

        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] SavingGoalDTO updatedGoal)
        {
            if (id != updatedGoal.Id)
                return BadRequest("Goal ID mismatch.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var updated = await _savingGoalsService.UpdateGoalAsync(userId, id, updatedGoal);
                if (updated == null)
                    return Forbid("Unauthorized or goal not found.");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating saving goal with ID {GoalId}", id);
                return StatusCode(500, "An error occurred while updating the goal.");
            }
        }

        [HttpPut("addtosavings/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddToSavings(int id, [FromBody] decimal amount)
        {
            if (amount <= 0)
                return BadRequest("Amount must be greater than 0.");

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var updated = await _savingGoalsService.AddToSavingsAsync(userId, id, amount);
                if (updated == null)
                    return Forbid("Unauthorized or goal not found.");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding amount to saving goal {GoalId}", id);
                return StatusCode(500, "An error occurred while adding to savings.");
            }
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            try
            {
                var result = await _savingGoalsService.DeleteGoalAsync(userId, id);
                if (!result)
                    return Forbid("Unauthorized or goal not found.");

                return Ok(new { message = "Goal deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting saving goal with ID {GoalId}", id);
                return StatusCode(500, "An error occurred while deleting the goal.");
            }
        }
    }
}
