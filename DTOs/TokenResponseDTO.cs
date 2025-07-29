namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public record TokenResponseDto
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public string Role { get; init; }
    }
}
