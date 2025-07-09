
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;

        public IncomesController(ExpenseTrackerDBContext context)
        {
            _context = context;
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateIncome(Income income)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            income.UserId = userId;

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return Ok(income);
        }

       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllIncomes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var incomes = await _context.Incomes
                                .Where(i => i.UserId == userId)
                                .ToListAsync();
            return Ok(incomes);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeById(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
                return NotFound();

            return Ok(income);
        }

        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateIncome(int id, Income updatedIncome)
        {
            if (id != updatedIncome.Id)
                return BadRequest("Income ID mismatch.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingIncome = await _context.Incomes.FindAsync(id);
            if (existingIncome == null)
                return NotFound();
            if (existingIncome.UserId.ToString() != userId)
                return Forbid("You are not allowed to update this income.");

  
            existingIncome.Date = updatedIncome.Date;
            existingIncome.Description = updatedIncome.Description;
            existingIncome.Amount = updatedIncome.Amount;
            existingIncome.Source = updatedIncome.Source;

            await _context.SaveChangesAsync();
            return Ok(existingIncome);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var income= await _context.Incomes.FindAsync(id);



            if (income == null)
            {
                return NotFound();
            }
            if (income.UserId.ToString() != userId)
                return Forbid("You are not allowed to delete this income.");

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();


            return Ok(new { message = "Income deleted successfully." });
        }

    }
}
