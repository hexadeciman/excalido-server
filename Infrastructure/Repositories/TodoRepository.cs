using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Todo> AddTodoAsync(Todo todo, int? index)
    {
        // Get the highest orderIndex in the list
        var maxOrderIndex = await _context.Todos
            .Where(t => t.ListId == todo.ListId)
            .Select(t => (int?)t.OrderIndex)
            .MaxAsync() ?? -1; // start from -1 so that the first todo gets index 0

        // Append the new todo at the end
        todo.OrderIndex = maxOrderIndex + 1;
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // If an insertion index is specified, reposition the newly added todo
        if (!index.HasValue) return todo;
        // Clamp the index to be within valid bounds (0 to count)
        var clampedIndex = Math.Clamp(index.Value + 1, 0, maxOrderIndex + 1);
        await ReorderTodos(todo.ListId, todo.OrderIndex, clampedIndex);

        return todo;
    }


    public async Task<Todo?> GetTodoByIdAsync(int id, string username)
    {
        return await _context.Todos
            .Where(t => t.Id == id && 
                        _context.Boards.Any(b => b.Id == t.List.BoardId && b.Owner.Username == username))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Todo>> GetTodosAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<Todo> UpdateTodoAsync(Todo todo, string username)
    {
        await GetTodoByIdAsync(todo.Id, username);
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
    
    public async Task ReorderTodos(int listId, int oldIndex, int newIndex)
    {
        var todos = await _context.Todos
            .Where(t => t.ListId == listId)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();

        var movedTodo = todos.FirstOrDefault(t => t.OrderIndex == oldIndex);
        if (movedTodo == null) return;

        // ✅ Remove the todo from the list
        todos.Remove(movedTodo);

        // ✅ Insert it at the new position
        todos.Insert(newIndex, movedTodo);

        // ✅ Reassign order indexes
        for (int i = 0; i < todos.Count; i++)
        {
            todos[i].OrderIndex = i;
        }

        await _context.SaveChangesAsync();
    }

    private async Task ShiftOrderIndexesAfterDeletion(int listId, int deletedOrderIndex)
    {
        var todosToShift = await _context.Todos
            .Where(t => t.ListId == listId && t.OrderIndex > deletedOrderIndex)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();

        foreach (var todo in todosToShift)
        {
            todo.OrderIndex--;
        }

        await _context.SaveChangesAsync();
    }
    public async Task<bool> DeleteTodoAsync(int id, string username)
    {
        var todo = await GetTodoByIdAsync(id, username);
        if (todo == null) return false;
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        
        await ShiftOrderIndexesAfterDeletion(todo.ListId, todo.OrderIndex);

        return true;
    }
}