namespace RestAPI.Contracts.Requests;

public class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? BusinessName { get; set; }
    public bool EmailVerified { get; set; } = false;
    public List<string> RoleIds { get; set; } = [];
}
