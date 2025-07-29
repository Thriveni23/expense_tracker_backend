using System.ComponentModel.DataAnnotations;
namespace ExpenseTrackerCrudWebAPI.Models
{
    public class Category
    {
        [Key]  
        public int Id { get; set; }

        [Required] 
        public string CategoryType { get; set; }
    }

}
