using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Board> AddBoardAsync(Board board, string username);
    Task<Board?> GetBoardByIdAsync(int id, string username);
    Task<List<Board>> GetAllBoardsAsync();

    Task<List<Board>> GetBoardsAsync(string username);
    Task<Board?> UpdateBoardAsync(Board board, int? orderIndex);
    Task<Board> GetBoardForUserAsync(int boardId, string username);
    Task<bool> DeleteBoardAsync(int id, string username);
}