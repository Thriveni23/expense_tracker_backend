namespace ExpenseTrackerCrudWebAPI.DTOs
{
	public class BudgetDTO
	{
		public int Id { get; set; }            // For update or response
		public string MonthYear { get; set; }  // Example: "July-2025"
		public string Category { get; set; }
		public decimal Amount { get; set; }

		// Optional: For multi-user/admin
		public string? UserId { get; set; }
	}
}
