namespace ExpenseTrackerCrudWebAPI.DTOs
{
	public class SavingGoalDTO
	{
		public int Id { get; set; }                // For update or response
		public string GoalName { get; set; }
		public decimal TargetAmount { get; set; }
		public decimal SavedAmount { get; set; }   // Useful for showing progress
		public string? UserId { get; set; }        // Include if needed for admin/multi-user
	}
}
