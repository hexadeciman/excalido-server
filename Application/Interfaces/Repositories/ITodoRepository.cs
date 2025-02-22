using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITodoRepository
{
    Task<Todo> AddTodoAsync(Todo todo);
    Task<Todo?> GetTodoByIdAsync(int id, string username);
    Task<List<Todo>> GetTodosAsync();
    Task<Todo> UpdateTodoAsync(Todo todo, string username);
    Task<bool> DeleteTodoAsync(int id, string username);
    Task ReorderTodos(int listId, int oldIndex, int newIndex);
}