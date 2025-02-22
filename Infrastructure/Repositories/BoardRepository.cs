using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BoardRepository(AppDbContext context, IUserRepository userRepository) : IBoardRepository
{
    public async Task<Board> AddBoardAsync(Board board)
    {
        context.Boards.Add(board);
        await context.SaveChangesAsync();
        return board;
    }


    public async Task<Board> AddBoardAsync(Board board, string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new Exception("User not found");

        board.OwnerId = user.Id;
        context.Boards.Add(board);
        await context.SaveChangesAsync();
        return board;
    }

    public async Task<List<Board>> GetBoardsAsync(string username)
    {
        return await context.Boards
            .Where(b => b.Owner.Username == username)
            .ToListAsync();
    }
    
    public async Task<Board?> GetBoardByIdAsync(int id)
    {
        return await context.Boards.FindAsync(id);
    }
    
    public async Task<Board> GetBoardForUserAsync(int boardId, string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if(user == null) throw new Exception("User not found");
        var board = await context.Boards.FirstOrDefaultAsync(b => b.Id == boardId && b.OwnerId == user.Id);
        if (board == null) throw new Exception("Board not found or does not belong to the user");
        return board;
    }

    public async Task<List<Board>> GetAllBoardsAsync()
    {
        return await context.Boards.ToListAsync();
    }
    


    public async Task<Board?> UpdateBoardAsync(Board board)
    {
        var existingBoard = await context.Boards.FindAsync(board.Id);
        if (existingBoard == null) return null;

        context.Entry(existingBoard).CurrentValues.SetValues(board);
        await context.SaveChangesAsync();
        return existingBoard;
    }

    public async Task<bool> DeleteBoardAsync(int id)
    {
        var board = await context.Boards.FindAsync(id);
        if (board == null) return false;

        context.Boards.Remove(board);
        await context.SaveChangesAsync();
        return true;
    }
}