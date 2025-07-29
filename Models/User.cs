using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace ExpenseTrackerCrudWebAPI.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
