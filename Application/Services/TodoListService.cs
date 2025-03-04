using Application.DTOs;
using Application.DTOs.Board;
using Application.DTOs.TodoList;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

public class TodoListService(ITodoListRepository todoListRepository)
{
    public async Task<TodoListDTO> CreateTodoListAsync(CreateTodoListDTO dto, string username)
        {
            var todoList = new ListEntity
            {
                Name = dto.Name,
                BoardId = dto.BoardId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            var createdTodoList = await todoListRepository.AddTodoListAsync(todoList, username);

            return new TodoListDTO
            {
                Id = createdTodoList.Id,
                Name = createdTodoList.Name,
                BoardId = createdTodoList.BoardId,
                CreatedAt = createdTodoList.CreatedAt,
                ModifiedAt = createdTodoList.ModifiedAt
            };
        }

        public async Task<TodoListDTO?> GetTodoListByIdAsync(int id, string username)
        {
            var todoList = await todoListRepository.GetTodoListByIdAsync(id, username);
            return new TodoListDTO
            {
                Id = todoList.Id,
                Name = todoList.Name,
                BoardId = todoList.BoardId,
                CreatedAt = todoList.CreatedAt,
                ModifiedAt = todoList.ModifiedAt
            };
        }

        public async Task<List<TodoListWithTodosDTO>> GetAllTodoListsAsync(int boardId, string username)
        {
            var todoLists = await todoListRepository.GetAllTodoListsAsync(boardId, username);
            return todoLists.Select(todoList => new TodoListWithTodosDTO
            {
                Id = todoList.Id,
                Name = todoList.Name,
                BoardId = todoList.BoardId,
                CreatedAt = todoList.CreatedAt,
                ModifiedAt = todoList.ModifiedAt,
                OrderIndex = todoList.XPosition,
                Todos = todoList.Todos.Select( t => new TodoDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    CreatedAt = t.CreatedAt,
                    Description = t.Description,
                    ListId = t.ListId,
                    ModifiedAt = t.ModifiedAt,
                    Status = $"{t.Status}",
                    OrderIndex = t.OrderIndex
                }).OrderBy(t => t.OrderIndex).ToList()
            }).OrderBy(l => l.OrderIndex).ToList();
        }
        
        public async Task<List<BoardWithTodoListWithTodosDTO>> GetBoardsAndTodoListsAsync(string username)
        {
            var todoLists = await todoListRepository.GetAllTodoListsAsync(username);
            return todoLists.Select(board => new BoardWithTodoListWithTodosDTO
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                TodoLists = board.Lists.Select( l => new TodoListWithTodosDTO
                {
                    Id = l.Id,
                    Name = l.Name,
                    BoardId = l.BoardId,
                    CreatedAt = l.CreatedAt,
                    ModifiedAt = l.ModifiedAt,
                    Todos = l.Todos.Select( t => new TodoDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        CreatedAt = t.CreatedAt,
                        Description = t.Description,
                        ListId = t.ListId,
                        ModifiedAt = t.ModifiedAt,
                        Status = $"{t.Status}",
                        OrderIndex = t.OrderIndex
                    }).OrderBy(t => t.OrderIndex).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<TodoListDTO?> UpdateTodoListAsync(int id, UpdateTodoListDTO dto, string username)
        {
            var modifiedTodoList = await todoListRepository.GetTodoListByIdAsync(id, username);

            if (!string.IsNullOrEmpty(dto.Name)) modifiedTodoList.Name = dto.Name;
            modifiedTodoList.ModifiedAt = DateTime.UtcNow;

            var updatedTodoList = await todoListRepository.UpdateTodoListAsync(modifiedTodoList, username);

            return new TodoListDTO
            {
                Id = updatedTodoList.Id,
                Name = updatedTodoList.Name,
                BoardId = updatedTodoList.BoardId,
                CreatedAt = updatedTodoList.CreatedAt,
                ModifiedAt = updatedTodoList.ModifiedAt
            };
        }
        
        public async Task<TodoListDTO?> ReorderTodoListAsync(int id, int newIndex, string username)
        {
            var updatedTodoList = await todoListRepository.ReorderTodoListAsync(id, newIndex, username);
            return new TodoListDTO
            {
                Id = updatedTodoList.Id,
                Name = updatedTodoList.Name,
                BoardId = updatedTodoList.BoardId,
                CreatedAt = updatedTodoList.CreatedAt,
                ModifiedAt = updatedTodoList.ModifiedAt
            };
        }
        
        public async Task<bool> DeleteTodoListAsync(int id, string username)
        {
            var modifiedTodoList = await todoListRepository.GetTodoListByIdAsync(id, username);
            return await todoListRepository.DeleteTodoListAsync(modifiedTodoList, username);
        }
}