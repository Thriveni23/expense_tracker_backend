using ExpenseTrackerCrudWebAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionDto, string userId);
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync(string userId);
        Task<TransactionDTO?> GetTransactionByIdAsync(int id, string userId);
        Task<TransactionDTO?> UpdateTransactionAsync(int id, TransactionDTO updatedDto, string userId);
        Task<bool> DeleteTransactionAsync(int id, string userId);
    }
}
