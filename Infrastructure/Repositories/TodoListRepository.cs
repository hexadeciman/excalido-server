using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TodoListRepository(AppDbContext context, IBoardRepository boardRepository) : ITodoListRepository
    {
        public async Task<ListEntity> AddTodoListAsync(ListEntity todoList, string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new Exception("User not found");

            var board = await context.Boards.FirstOrDefaultAsync(b => b.Id == todoList.BoardId && b.OwnerId == user.Id);
            if (board == null) throw new Exception("Board not found");

            context.Lists.Add(todoList);
            await context.SaveChangesAsync();
            return todoList;
        }

        public async Task<ListEntity> GetTodoListByIdAsync(int id, string username)
        {
            var list = await context.Lists
                .Include(l => l.Board)
                .ThenInclude(b => b.Owner)
                .FirstOrDefaultAsync(l => l.Id == id && l.Board.Owner.Username == username);
            if(list == null) throw new Exception("List not found");
            return list;
        }

        public async Task<ListEntity?> GetTodoListByIdAsync(int id)
        {
            return await context.Lists.FindAsync(id);
        }

        public async Task<List<ListEntity>> GetAllTodoListsAsync(int boardId, string username)
        {
            var board = await context.Boards
                .Where(b => b.Id == boardId && b.Owner.Username == username)
                .FirstOrDefaultAsync();

            if (board == null)
                return new List<ListEntity>(); // Return empty list if board doesn't belong to the user

            return await context.Lists
                .Where(l => l.BoardId == boardId)
                .ToListAsync();
        }
        
        public async Task<List<Board>> GetAllTodoListsAsync(string username) => await context.Boards
                .Where(b => b.Owner.Username == username)
                .Include(b => b.Lists)
                .ToListAsync();
        
        public async Task<ListEntity> UpdateTodoListAsync(ListEntity todoList, string username)
        {
            await boardRepository.GetBoardForUserAsync(todoList.BoardId, username);
            context.Lists.Update(todoList);
            await context.SaveChangesAsync();
            return todoList;
        }

        public async Task<bool> DeleteTodoListAsync(ListEntity todoList, string username)
        {
            await boardRepository.GetBoardForUserAsync(todoList.BoardId, username);
            context.Lists.Remove(todoList);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ListEntity> ReorderTodoListAsync(int id, int newIndex, string username)
        {
            // Fetch the todo list and ensure the user has access
            var todoList = await context.Lists
                .Include(l => l.Board)
                .ThenInclude(b => b.Owner)
                .FirstOrDefaultAsync(l => l.Id == id && l.Board.Owner.Username == username);

            if (todoList == null)
                throw new Exception("Todo list not found or access denied");

            // Fetch all lists for the board in order
            var lists = await context.Lists
                .Where(l => l.BoardId == todoList.BoardId)
                .OrderBy(l => l.XPosition)
                .ToListAsync();

            var oldIndex = lists.FindIndex(l => l.Id == id);
            if (oldIndex == -1)
                throw new Exception("Todo list not found in ordering");

            // Ensure the new index is within bounds
            newIndex = Math.Clamp(newIndex, 0, lists.Count - 1);

            // Remove the list from its old position
            lists.RemoveAt(oldIndex);

            // Insert it at the new position
            lists.Insert(newIndex, todoList);

            // Reassign order indexes
            for (var i = 0; i < lists.Count; i++)
            {
                lists[i].XPosition = i;
                context.Lists.Update(lists[i]); // ✅ Explicitly mark the entity as updated
            }

            await context.SaveChangesAsync();
            return todoList;
        }
    }
}
