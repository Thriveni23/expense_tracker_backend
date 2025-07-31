using ExpenseTrackerCrudWebAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTrackerCrudWebAPI.Helpers;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionDto, string userId);
        Task<PaginatedResult<TransactionDTO>> GetAllTransactionsAsync(string userId, PaginationParamsDto paginationParams);

        Task<TransactionDTO?> GetTransactionByIdAsync(int id, string userId);
        Task<TransactionDTO?> UpdateTransactionAsync(int id, TransactionDTO updatedDto, string userId);
        Task<bool> DeleteTransactionAsync(int id, string userId);
    }
}
