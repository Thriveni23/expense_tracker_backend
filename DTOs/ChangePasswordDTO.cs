namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public record ChangePasswordDTO
    {
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
    }
}
