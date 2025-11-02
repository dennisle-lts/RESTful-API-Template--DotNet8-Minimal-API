using Microsoft.AspNetCore.Mvc;
using RestAPI.Contracts.Responses;
using RestAPI.Services.Interfaces;

namespace RestAPI.Endpoints.User;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/user").WithTags("User");
        group
            .MapGet("/{userId}", GetMyProfile)
            .WithName("Update My Profile")
            .WithSummary("Update current user profile information")
            .WithDescription("Updates the profile information of the currently authenticated user.")
            .Produces<UserResponse>(200, "application/json")
            .Produces<ProblemDetails>(400, "application/json")
            .Produces<ProblemDetails>(401, "application/json")
            .Produces<ProblemDetails>(500, "application/json");
    }

    private static async Task<IResult> GetMyProfile(
        string userId,
        HttpContext context,
        IUserService userService
    )
    {
        var response = await userService.GetUserById(userId);

        return Results.Ok(response);
    }
}
