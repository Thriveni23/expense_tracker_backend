namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public record UserDTO
    {
        public string Id { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Role { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
