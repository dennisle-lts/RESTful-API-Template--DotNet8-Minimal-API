using RestAPI.Contracts.Responses;

namespace RestAPI.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> GetUserById(string userId);
    Task<UserResponse> GetUserByEmail(string email);
}
