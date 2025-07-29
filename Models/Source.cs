using System.ComponentModel.DataAnnotations;
namespace ExpenseTrackerCrudWebAPI.Models
{
    public class Source
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SourceType { get; set; }
    }

}
