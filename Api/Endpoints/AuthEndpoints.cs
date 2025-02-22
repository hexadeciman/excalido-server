using Application.DTOs;
using Application.Services;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    private readonly record struct StringResponse(string Message);

    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithOpenApi()
            .WithTags("Auth");

        group.MapPost("/signup", async (RegisterRequestDTO request, AuthService authService) =>
        {
            var user = await authService.RegisterUserAsync(request);
            return Results.Created($"/api/auth/users/{user.Id}", user);
        }).Produces<UserDTO>(201);

        group.MapPost("/login", async (AuthRequestDTO request, AuthService authService) =>
        {
            var authResponse = await authService.AuthenticateUserAsync(request);
            return authResponse is not null ? Results.Ok(authResponse) : Results.Unauthorized();
        }).Produces<AuthResponseDTO>().Produces(401);

        group.MapPost("/logout", () => Results.Ok(new StringResponse("Logout successful"))).Produces<StringResponse>();
    }
}