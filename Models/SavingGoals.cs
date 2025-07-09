using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerCrudWebAPI.Models
{
    public class SavingGoals
    {
        [Key]
        public int Id { get; set; }

       
        public string GoalName { get; set; } = string.Empty;

      
        public decimal TargetAmount { get; set; }

        public decimal SavedAmount { get; set; }

       
        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
