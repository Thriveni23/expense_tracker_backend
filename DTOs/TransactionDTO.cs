using System;

namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }         // For update/response
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }

        // Optional: Only if admin needs to see the user
        public string? UserId { get; set; }
    }
}
