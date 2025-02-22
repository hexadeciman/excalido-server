using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITodoListRepository
{
    Task<ListEntity> AddTodoListAsync(ListEntity todoList, string username);
    Task<ListEntity> GetTodoListByIdAsync(int id, string username);
    Task<List<ListEntity>> GetAllTodoListsAsync(int boardId, string username);
    Task<ListEntity> UpdateTodoListAsync(ListEntity todoList, string username);
    Task<bool> DeleteTodoListAsync(ListEntity todoList, string username);
}