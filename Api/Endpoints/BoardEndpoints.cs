using Application.DTOs.Board;
using Application.Services;

namespace Api.Endpoints;

public static class BoardEndpoints
{
    public static void MapBoardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/board").WithOpenApi().WithTags("Board").RequireAuthorization();

        group.MapPost("/", async (CreateBoardDTO request, BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.User.Identity?.Name;
            if (username == null)
            {
                return Results.NotFound();    
            }
            var board = await boardService.CreateBoardAsync(request, username);
            return Results.Created($"/boards/{board.Id}", board);    
               
        });

        group.MapGet("/", async (BoardService boardService, HttpContext httpContext) =>
        {
            var username = httpContext.User.Identity?.Name;
            if (username == null)
            {
                return Results.NotFound();    
            }
            var boards = await boardService.GetBoardsAsync(username);
            return Results.Ok(boards);
        });

        group.MapGet("/{id:int}", async (int id, BoardService boardService) =>
        {
            var board = await boardService.GetBoardByIdAsync(id);
            return board is not null ? Results.Ok(board) : Results.NotFound();
        });

    }
}

