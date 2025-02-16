using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITodoListRepository
{
    Task<ListEntity> AddTodoListAsync(ListEntity todoList);
    Task<ListEntity?> GetTodoListByIdAsync(int id);
    Task<List<ListEntity>> GetAllTodoListsAsync();
    Task<ListEntity> UpdateTodoListAsync(ListEntity todoList);
    Task<bool> DeleteTodoListAsync(int id);
}