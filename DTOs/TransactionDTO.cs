using System;
using System.ComponentModel.DataAnnotations;
using ExpenseTrackerCrudWebAPI.Attributes;


namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }      
   
        [BindAndRequired]
        public DateTime Date { get; set; }

        [BindAndRequired]
        public string Description { get; set; }

        [BindAndRequired]
        public decimal Amount { get; set; }

        [BindAndRequired]
        public string Category { get; set; }

     
        public string? UserId { get; set; }
    }
}
