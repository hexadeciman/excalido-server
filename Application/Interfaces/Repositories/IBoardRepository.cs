using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Board> AddBoardAsync(Board board, string username);
    Task<Board?> GetBoardByIdAsync(int id);
    Task<List<Board>> GetAllBoardsAsync();

    Task<List<Board>> GetBoardsAsync(string username);
    Task<Board?> UpdateBoardAsync(Board board);
    Task<bool> DeleteBoardAsync(int id);
}