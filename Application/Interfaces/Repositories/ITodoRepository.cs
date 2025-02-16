using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITodoRepository
{
    Task<Todo> AddTodoAsync(Todo todo);
    Task<Todo?> GetTodoByIdAsync(int id);
    Task<List<Todo>> GetTodosAsync();
    Task<Todo> UpdateTodoAsync(Todo todo);
    Task<bool> DeleteTodoAsync(int id);
}