using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        // Register services
        services.AddScoped<TodoService>();
        services.AddScoped<BoardService>();
        services.AddScoped<TodoListService>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();

        return services;
    }
}