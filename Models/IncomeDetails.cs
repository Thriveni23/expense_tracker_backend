using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerCrudWebAPI.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
      
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }


    }
}
