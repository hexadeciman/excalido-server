using Api.Middleware;
using Application.DTOs.Board;
using Application.Services;

namespace Api.Endpoints;

public static class BoardEndpoints
{
    public static void MapBoardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/board")
            .WithOpenApi()
            .WithTags("Board")
            .RequireAuthorization()
            .AddEndpointFilter<RequireAuthenticatedUserFilter>();

        group.MapPost("/", async (CreateBoardDTO request, BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var board = await boardService.CreateBoardAsync(request, username!);
            return Results.Created($"/boards/{board.Id}", board);

        }).Produces<BoardDTO>(201).Produces(401).Produces(500);

        group.MapGet("/", async (BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var boards = await boardService.GetBoardsAsync(username!);
            return Results.Ok(boards);
        }).Produces<List<BoardDTO>>().Produces(401).Produces(500);

        group.MapGet("/{id:int}", async (int id, BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var board = await boardService.GetBoardByIdAsync(id, username!);
            return board is not null ? Results.Ok(board) : Results.NotFound();
        }).Produces<BoardDTO>().Produces(401).Produces(500);
        
        group.MapPut("/", async (UpdateBoardDTO request, BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.Items["Username"] as string;
            var board = await boardService.UpdateBoardAsync(request, username!);
            return board is not null ? Results.Ok(board) : Results.NotFound();
        }).Produces<BoardDTO>().Produces(401).Produces(500);

    }
}

