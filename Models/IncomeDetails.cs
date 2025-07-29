using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerCrudWebAPI.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Source { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }


    }
}
