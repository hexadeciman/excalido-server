using Application.DTOs;
using Application.Services;

namespace Api.Endpoints;

public static class TodosEndpoints
{
    public static void MapTodosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo").WithOpenApi().WithTags("Todo").RequireAuthorization();

        group.MapPost("/", async (CreateTodoDTO request, TodoService todoService) =>
        {
            var todo = await todoService.CreateTodoAsync(request);
            return Results.Created($"/todos/{todo.Id}", todo);
        });

        group.MapGet("/", async (TodoService todoService) =>
        {
            var todos = await todoService.GetAllTodosAsync();
            return Results.Ok(todos);
        });

        group.MapGet("/{id:int}", async (int id, TodoService todoService) =>
        {
            var todo = await todoService.GetTodoByIdAsync(id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        });

        group.MapPut("/{id:int}", async (int id, UpdateTodoDTO request, TodoService todoService) =>
        {
            var updatedTodo = await todoService.UpdateTodoAsync(id, request);
            return updatedTodo is not null ? Results.Ok(updatedTodo) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, TodoService todoService) =>
        {
            var deleted = await todoService.DeleteTodoAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}