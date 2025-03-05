using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BoardRepository(AppDbContext context, IUserRepository userRepository) : GenericRepository<Board>(context), IBoardRepository
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

        var maxOrderIndex = await context.Boards
            .Where(b => b.OwnerId == user.Id)
            .MaxAsync(b => (int?)b.OrderIndex) ?? -1;

        board.OwnerId = user.Id;
        board.OrderIndex = maxOrderIndex + 1; // Ensure new board gets the next order index

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
    
    public async Task<Board?> GetBoardByIdAsync(int id, string username)
    {
        return await context.Boards.Where(b => b.Owner.Username == username && b.Id == id).FirstOrDefaultAsync();
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
    


    public async Task<Board?> UpdateBoardAsync(Board board, int? orderIndex)
    {
        var existingBoard = await context.Boards.FindAsync(board.Id);
        if (existingBoard == null) return null;

        existingBoard.Name = board.Name;

        if (orderIndex != null)
        {
            await ReorderEntitiesAsync(
                b => b.Id == board.Id,
                b => b.OrderIndex,
                (b, index) => b.OrderIndex = index,
                board.OwnerId,
                (int)orderIndex!
            );
        }
        else
        {
            context.Boards.Update(existingBoard);
        }

        await context.SaveChangesAsync();
        return existingBoard;
    }

    public async Task<bool> DeleteBoardAsync(int id, string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if(user == null) throw new Exception("User not found");

        var board = await context.Boards.FindAsync(id);
        if (board == null) return false;

        context.Boards.Remove(board);
        await context.SaveChangesAsync();
        return true;
    }
}