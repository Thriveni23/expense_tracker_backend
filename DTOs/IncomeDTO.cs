using System;

namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public class IncomeDTO
    {
        public int Id { get; set; }           // For update or response
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }

        // Optional: Include only if you need to show user ownership (e.g., admin view)
        public string? UserId { get; set; }
    }
}
