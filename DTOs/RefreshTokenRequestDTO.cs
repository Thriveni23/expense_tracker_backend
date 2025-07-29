namespace ExpenseTrackerCrudWebAPI.DTOs
{
    public record RefreshTokenRequestDto
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
