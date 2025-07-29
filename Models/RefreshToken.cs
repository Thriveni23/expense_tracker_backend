using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerCrudWebAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}