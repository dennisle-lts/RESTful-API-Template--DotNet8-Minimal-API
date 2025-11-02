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
            .WithName("Get User By Id")
            .WithSummary("Retrieve user information by id")
            .WithDescription("Returns the information of the specified user.")
            .Produces<UserResponse>(200, "application/json")
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
