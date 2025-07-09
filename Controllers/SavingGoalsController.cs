using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingGoalsController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;

        public SavingGoalsController(ExpenseTrackerDBContext context)
        {
            _context = context;
        }

 
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateGoal(SavingGoals goal)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            goal.UserId = userId;

            _context.SavingGoals.Add(goal);
            await _context.SaveChangesAsync();

            return Ok(goal);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllGoals()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var goals = await _context.SavingGoals
                                      .Where(g => g.UserId == userId)
                                      .ToListAsync();

            return Ok(goals);
        }

      
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetGoalById(int id)
        {
            var goal = await _context.SavingGoals.FindAsync(id);
            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGoal(int id, SavingGoals updatedGoal)
        {
            if (id != updatedGoal.Id)
                return BadRequest("Goal ID mismatch.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingGoal = await _context.SavingGoals.FindAsync(id);
            if (existingGoal == null)
                return NotFound();

            if (existingGoal.UserId != userId)
                return Forbid("You are not allowed to update this goal.");

          
            existingGoal.GoalName = updatedGoal.GoalName;
            existingGoal.TargetAmount = updatedGoal.TargetAmount;
            existingGoal.SavedAmount = updatedGoal.SavedAmount;

            await _context.SaveChangesAsync();

            return Ok(existingGoal);
        }

        [HttpPut("addtosavings/{id}")]
        [Authorize]
        public async Task<IActionResult> AddToSavings(int id, [FromBody] decimal amount)
        {
            if (amount <= 0) return BadRequest();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var goal = await _context.SavingGoals.FindAsync(id);
            if (goal == null || goal.UserId != userId) return Unauthorized();

            goal.SavedAmount += amount;
            await _context.SaveChangesAsync();

            return Ok(goal);
        }


       

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var goal = await _context.SavingGoals.FindAsync(id);
            if (goal == null)
                return NotFound();

            if (goal.UserId != userId)
                return Forbid("You are not allowed to delete this goal.");

            _context.SavingGoals.Remove(goal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Saving goal deleted successfully." });
        }
    }
}
