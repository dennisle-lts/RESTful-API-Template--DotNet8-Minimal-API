using RestAPI.Contracts.Responses;
using RestAPI.Domain;

namespace RestAPI.Mapping;

public static class UserMapping
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Name = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Phone = user.Phone,
            BusinessName = user.BusinessName,
            EmailVerified = user.EmailVerified,
            JoinedSince = user.CreatedAt,
        };
    }
}
