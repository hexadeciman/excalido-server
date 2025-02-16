using Application.DTOs.TodoList;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

public class TodoListService
{
    private readonly ITodoListRepository _todoListRepository;

        public TodoListService(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<TodoListDTO> CreateTodoListAsync(CreateTodoListDTO dto)
        {
            var todoList = new ListEntity
            {
                Name = dto.Name,
                BoardId = dto.BoardId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            var createdTodoList = await _todoListRepository.AddTodoListAsync(todoList);

            return new TodoListDTO
            {
                Id = createdTodoList.Id,
                Name = createdTodoList.Name,
                BoardId = createdTodoList.BoardId,
                CreatedAt = createdTodoList.CreatedAt,
                ModifiedAt = createdTodoList.ModifiedAt
            };
        }

        public async Task<TodoListDTO?> GetTodoListByIdAsync(int id)
        {
            var todoList = await _todoListRepository.GetTodoListByIdAsync(id);
            return todoList == null ? null : new TodoListDTO
            {
                Id = todoList.Id,
                Name = todoList.Name,
                BoardId = todoList.BoardId,
                CreatedAt = todoList.CreatedAt,
                ModifiedAt = todoList.ModifiedAt
            };
        }

        public async Task<List<TodoListDTO>> GetAllTodoListsAsync()
        {
            var todoLists = await _todoListRepository.GetAllTodoListsAsync();
            return todoLists.Select(todoList => new TodoListDTO
            {
                Id = todoList.Id,
                Name = todoList.Name,
                BoardId = todoList.BoardId,
                CreatedAt = todoList.CreatedAt,
                ModifiedAt = todoList.ModifiedAt
            }).ToList();
        }

        public async Task<TodoListDTO?> UpdateTodoListAsync(int id, UpdateTodoListDTO dto)
        {
            var existingTodoList = await _todoListRepository.GetTodoListByIdAsync(id);
            if (existingTodoList == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) existingTodoList.Name = dto.Name;
            existingTodoList.ModifiedAt = DateTime.UtcNow;

            var updatedTodoList = await _todoListRepository.UpdateTodoListAsync(existingTodoList);

            return new TodoListDTO
            {
                Id = updatedTodoList.Id,
                Name = updatedTodoList.Name,
                BoardId = updatedTodoList.BoardId,
                CreatedAt = updatedTodoList.CreatedAt,
                ModifiedAt = updatedTodoList.ModifiedAt
            };
        }

        public async Task<bool> DeleteTodoListAsync(int id)
        {
            return await _todoListRepository.DeleteTodoListAsync(id);
        }
}