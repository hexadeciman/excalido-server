using Application.DTOs.TodoList;
using Application.Services;
namespace Api.Endpoints;

public static class TodoListEndpoints
{
    public static void MapTodoListEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/boards/{boardId:int}/lists")
            .WithOpenApi()
            .WithTags("TodoList")
            .RequireAuthorization();

        group.MapPost("/", async (int boardId, CreateTodoListDTO request, TodoListService todoListService) =>
        {
            request.BoardId = boardId;
            var todoList = await todoListService.CreateTodoListAsync(request);
            return Results.Created($"/api/boards/{boardId}/lists/{todoList.Id}", todoList);
        });

        group.MapGet("/", async (int boardId, TodoListService todoListService) =>
        {
            var todoLists = await todoListService.GetAllTodoListsAsync();
            return Results.Ok(todoLists);
        });

        group.MapGet("/{id:int}", async (int boardId, int id, TodoListService todoListService) =>
        {
            var todoList = await todoListService.GetTodoListByIdAsync(id);
            return todoList is not null ? Results.Ok(todoList) : Results.NotFound();
        });

        group.MapPut("/{id:int}", async (int boardId, int id, UpdateTodoListDTO request, TodoListService todoListService) =>
        {
            var updatedTodoList = await todoListService.UpdateTodoListAsync(id, request);
            return updatedTodoList is not null ? Results.Ok(updatedTodoList) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int boardId, int id, TodoListService todoListService) =>
        {
            var deleted = await todoListService.DeleteTodoListAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}