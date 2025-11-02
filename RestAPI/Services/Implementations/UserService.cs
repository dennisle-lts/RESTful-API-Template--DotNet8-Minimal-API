using RestAPI.Contracts.Responses;
using RestAPI.Mapping;
using RestAPI.Repositories.Interfaces;
using RestAPI.Services.Interfaces;

namespace RestAPI.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponse> GetUserById(string userId)
    {
        var user =
            await _userRepository.GetByIdAsync(userId)
            ?? throw new ArgumentException("User not found");

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is disabled");
        }

        _logger.LogInformation("Retrieved data for user {UserId}", userId);

        return user.ToUserResponse();
    }

    public async Task<UserResponse> GetUserByEmail(string email)
    {
        var user =
            await _userRepository.GetByEmailAsync(email)
            ?? throw new ArgumentException("User not found");

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is disabled");
        }

        _logger.LogInformation("Retrieved data for user {UserId}", user.Id);

        return user.ToUserResponse();
    }
}
