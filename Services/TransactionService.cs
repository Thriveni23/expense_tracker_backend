using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Helpers;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionDto, string userId)
        {
            var transaction = new Transaction
            {
                Date = transactionDto.Date,
                Description = transactionDto.Description,
                Amount = transactionDto.Amount,
                Category = transactionDto.Category,
                UserId = userId
            };

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();

            

            return transactionDto;
        }

        public async Task<PaginatedResult<TransactionDTO>> GetAllTransactionsAsync(string userId, PaginationParamsDto paginationParams)
        {
            // Get filtered query
            var query = _unitOfWork.Transactions.GetQueryable().Where(t => t.UserId == userId);

            // Total count for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var transactions = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            // Map to DTOs
            var transactionDtos = transactions.Select(t => new TransactionDTO
            {
                Id = t.Id,
                Date = t.Date,
                Description = t.Description,
                Amount = t.Amount,
                Category = t.Category,
                UserId = t.UserId
            }).ToList();

            // Return paginated result
            return new PaginatedResult<TransactionDTO>(transactionDtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }


        public async Task<TransactionDTO?> GetTransactionByIdAsync(int id, string userId)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null || transaction.UserId != userId)
                return null;

            return new TransactionDTO
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Category = transaction.Category,
                UserId = transaction.UserId
            };
        }

        public async Task<TransactionDTO?> UpdateTransactionAsync(int id, TransactionDTO updatedDto, string userId)
        {
            var existing = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return null;

            existing.Description = updatedDto.Description;
            existing.Amount = updatedDto.Amount;
            existing.Category = updatedDto.Category;
            existing.Date = updatedDto.Date;

            _unitOfWork.Transactions.Update(existing);
            await _unitOfWork.CompleteAsync();

            return new TransactionDTO
            {
                Id = existing.Id,
                Date = existing.Date,
                Description = existing.Description,
                Amount = existing.Amount,
                Category = existing.Category,
                UserId = existing.UserId
            };
        }

        public async Task<bool> DeleteTransactionAsync(int id, string userId)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null || transaction.UserId != userId)
                return false;

            _unitOfWork.Transactions.Delete(transaction);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}