using Api.Middleware;
using Application.DTOs;
using Application.Services;

namespace Api.Endpoints;

public static class TodosEndpoints
{
    public static void MapTodosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo").WithOpenApi().WithTags("Todo")
            .RequireAuthorization()
            .WithOpenApi()
            .WithTags("Todo")
            .RequireAuthorization()
            .AddEndpointFilter<RequireAuthenticatedUserFilter>();

        group.MapPost("/", async (CreateTodoDTO request, TodoService todoService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var todo = await todoService.CreateTodoAsync(request, username!);
            return Results.Created($"/todos/{todo.Id}", todo);
        }).Produces<List<TodoDTO>>(200).Produces(401).Produces(500);

        group.MapGet("/", async (TodoService todoService) =>
        {
            var todos = await todoService.GetAllTodosAsync();
            return Results.Ok(todos);
        }).Produces<List<List<TodoDTO>>>(200).Produces(401).Produces(500);

        group.MapGet("/{id:int}", async (int id, TodoService todoService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var todo = await todoService.GetTodoByIdAsync(id, username!);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        }).Produces<TodoDTO>(200).Produces(401).Produces(500);

        group.MapPut("/{id:int}", async (int id, UpdateTodoDTO request, TodoService todoService, HttpContext HttpContext) =>
        {
            var username = HttpContext.Items["Username"] as string;
            var updatedTodo = await todoService.UpdateTodoAsync(id, request, username!);
            return updatedTodo is not null ? Results.Ok(updatedTodo) : Results.NotFound();
        }).Produces<TodoDTO>(200).Produces(401).Produces(500);

        group.MapDelete("/{id:int}", async (int id, TodoService todoService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var deleted = await todoService.DeleteTodoAsync(id, username!);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).Produces(204).Produces(401).Produces(500);;
    }
}