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
    }
}
