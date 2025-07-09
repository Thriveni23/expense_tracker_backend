using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerCrudWebAPI.Models
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }
       
        public string MonthYear { get; set; }
       
        public string Category { get; set; }
        public decimal Amount { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
