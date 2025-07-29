using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Interfaces;
using ExpenseTrackerCrudWebAPI.Services;
using AutoMapper;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO transactionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("User {UserId} is creating a transaction of amount {Amount} in category {Category}", userId, transactionDto.Amount, transactionDto.Category);

            try
            {

                var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto, userId);
                return Ok(createdTransaction); // Already DTO
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction for user {UserId}", userId);
                return StatusCode(500, "An error occurred while creating the transaction.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync(userId);
                return Ok(transactions); // Already a list of DTOs
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching transactions for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving transactions.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id, userId);
                if (transaction == null) return NotFound();

                return Ok(transaction); // Already DTO
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while retrieving the transaction.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDTO transactionDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            try
            {
                var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transactionDto, userId);
                if (updatedTransaction == null) return Forbid("You are not allowed to update this transaction or it does not exist.");

                return Ok(updatedTransaction); // Already DTO
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while updating the transaction.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            try
            {
                var success = await _transactionService.DeleteTransactionAsync(id, userId);
                if (!success) return Forbid("You are not allowed to delete this transaction or it does not exist.");

                return Ok(new { message = "Transaction deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while deleting the transaction.");
            }
        }
    }
}