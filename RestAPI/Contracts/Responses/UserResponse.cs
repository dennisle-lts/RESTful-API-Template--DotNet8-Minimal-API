namespace RestAPI.Contracts.Responses;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? BusinessName { get; set; }
    public bool EmailVerified { get; set; } = false;
    public DateTime JoinedSince { get; set; }
}
