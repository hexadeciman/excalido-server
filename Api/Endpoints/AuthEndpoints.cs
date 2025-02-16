using Application.DTOs;
using Application.Services;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithOpenApi()
            .WithTags("Auth");

        group.MapPost("/signup", async (RegisterRequestDTO request, AuthService authService) =>
        {
            var user = await authService.RegisterUserAsync(request);
            return Results.Created($"/api/auth/users/{user.Id}", user);
        });

        group.MapPost("/login", async (AuthRequestDTO request, AuthService authService) =>
        {
            var authResponse = await authService.AuthenticateUserAsync(request);
            return authResponse is not null ? Results.Ok(authResponse) : Results.Unauthorized();
        });

        group.MapPost("/logout", () =>
        {
            return Results.Ok(new { Message = "Logout successful" });
        });
    }
}