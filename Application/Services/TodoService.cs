using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
namespace Application.Services;

public class TodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<TodoDTO> CreateTodoAsync(CreateTodoDTO dto)
    {
        var todo = new Todo
        {
            ListId = dto.ListId,
            Name = dto.Name,
            Description = dto.Description,
            Status = TodoStatusEnum.Unchecked,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        var createdTodo = await _todoRepository.AddTodoAsync(todo);

        return new TodoDTO
        {
            Id = createdTodo.Id,
            ListId = createdTodo.ListId,
            Name = createdTodo.Name,
            Description = createdTodo.Description,
            Status = createdTodo.Status.ToString(),
            CreatedAt = createdTodo.CreatedAt,
            ModifiedAt = createdTodo.ModifiedAt
        };
    }

    public async Task<TodoDTO?> GetTodoByIdAsync(int id)
    {
        var todo = await _todoRepository.GetTodoByIdAsync(id);
        return todo == null
            ? null
            : new TodoDTO
            {
                Id = todo.Id,
                ListId = todo.ListId,
                Name = todo.Name,
                Description = todo.Description,
                Status = todo.Status.ToString(),
                CreatedAt = todo.CreatedAt,
                ModifiedAt = todo.ModifiedAt
            };
    }

    public async Task<List<TodoDTO>> GetAllTodosAsync()
    {
        var todos = await _todoRepository.GetTodosAsync();
        return todos.Select(todo => new TodoDTO
        {
            Id = todo.Id,
            ListId = todo.ListId,
            Name = todo.Name,
            Description = todo.Description,
            Status = todo.Status.ToString(),
            CreatedAt = todo.CreatedAt,
            ModifiedAt = todo.ModifiedAt
        }).ToList();
    }

    public async Task<TodoDTO?> UpdateTodoAsync(int id, UpdateTodoDTO dto)
    {
        var existingTodo = await _todoRepository.GetTodoByIdAsync(id);
        if (existingTodo == null) return null;

        if (!string.IsNullOrEmpty(dto.Name)) existingTodo.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Description)) existingTodo.Description = dto.Description;
        if (!string.IsNullOrEmpty(dto.Status) && Enum.TryParse(dto.Status, out TodoStatusEnum status))
            existingTodo.Status = status;

        existingTodo.ModifiedAt = DateTime.UtcNow;

        var updatedTodo = await _todoRepository.UpdateTodoAsync(existingTodo);

        return new TodoDTO
        {
            Id = updatedTodo.Id,
            ListId = updatedTodo.ListId,
            Name = updatedTodo.Name,
            Description = updatedTodo.Description,
            Status = updatedTodo.Status.ToString(),
            CreatedAt = updatedTodo.CreatedAt,
            ModifiedAt = updatedTodo.ModifiedAt
        };
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        return await _todoRepository.DeleteTodoAsync(id);
    }
}
