using Api.Middleware;
using Application.DTOs.Board;
using Application.DTOs.TodoList;
using Application.Services;
namespace Api.Endpoints;

public static class TodoListEndpoints
{
    public static void MapTodoListEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todoList")
            .WithOpenApi()
            .WithTags("TodoList")
            .RequireAuthorization()
            .AddEndpointFilter<RequireAuthenticatedUserFilter>();


        group.MapPost("/", async (CreateTodoListDTO request, TodoListService todoListService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var todoList = await todoListService.CreateTodoListAsync(request, username!);
            return Results.Ok(todoList);
        }).Produces<List<TodoListDTO>>(200).Produces(401).Produces(500);

        group.MapGet("/{boardId:int}", async (int boardId, TodoListService todoListService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var todoLists = await todoListService.GetAllTodoListsAsync(boardId, username!);
            return Results.Ok(todoLists);
        }).Produces<List<TodoListWithTodosDTO>>(200).Produces(401).Produces(500);;

        group.MapGet("/", async (TodoListService todoListService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var boardWithTodoLists = await todoListService.GetBoardsAndTodoListsAsync(username!);
            return Results.Ok(boardWithTodoLists);
        }).Produces<List<BoardWithTodoListWithTodosDTO>>(200).Produces(401).Produces(500);;

        
        group.MapPut("/", async (int todoListId, UpdateTodoListDTO request, TodoListService todoListService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var updatedTodoList = await todoListService.UpdateTodoListAsync(todoListId, request, username!);
            return updatedTodoList is not null ? Results.Ok(updatedTodoList) : Results.NotFound();
        }).Produces<TodoListDTO>(200).Produces(401).Produces(500);

        group.MapDelete("/", async (int id, TodoListService todoListService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var deleted = await todoListService.DeleteTodoListAsync(id, username!);
            return deleted ? Results.NoContent() : Results.NotFound();
        }).Produces<bool>(200).Produces(401).Produces(500);
    }
}