using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using ExpenseTrackerCrudWebAPI.Models;
using System.Security.Claims;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;

        public TransactionsController(ExpenseTrackerDBContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            transaction.UserId = userId;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }


      
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var transactions = await _context.Transactions
                                .Where(i => i.UserId == userId)
                                .ToListAsync();
            return Ok(transactions);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

      
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction updatedTransaction)
        {
            if (id != updatedTransaction.Id)
                return BadRequest("Transaction ID mismatch.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingTransaction = await _context.Transactions.FindAsync(id);
            if (existingTransaction == null)
                return NotFound();
            if (existingTransaction.UserId.ToString() != userId)
                return Forbid("You are not allowed to update this income.");

           
            existingTransaction.Date = updatedTransaction.Date;
            existingTransaction.Description = updatedTransaction.Description;
            existingTransaction.Amount = updatedTransaction.Amount;
            existingTransaction.Category = updatedTransaction.Category;

            await _context.SaveChangesAsync();
            return Ok(existingTransaction);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }
            if (transaction.UserId.ToString() != userId)
                return Forbid("You are not allowed to delete this income.");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

           
            return Ok(new { message = "Transaction deleted successfully." });
        }
    }
}
