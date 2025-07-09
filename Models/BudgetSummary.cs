namespace ExpenseTrackerCrudWebAPI.Models
{
    public class BudgetSummary
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }      
        public decimal Spent { get; set; }      
        public decimal Remaining { get; set; }   
    }
}
