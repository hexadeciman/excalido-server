using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TodoListRepository(AppDbContext context) : ITodoListRepository
    {
        public async Task<ListEntity> AddTodoListAsync(ListEntity todoList)
        {
            context.Lists.Add(todoList);
            await context.SaveChangesAsync();
            return todoList;
        }

        public async Task<ListEntity?> GetTodoListByIdAsync(int id)
        {
            return await context.Lists.FindAsync(id);
        }

        public async Task<List<ListEntity>> GetAllTodoListsAsync()
        {
            return await context.Lists.ToListAsync();
        }

        public async Task<ListEntity> UpdateTodoListAsync(ListEntity todoList)
        {
            context.Lists.Update(todoList);
            await context.SaveChangesAsync();
            return todoList;
        }

        public async Task<bool> DeleteTodoListAsync(int id)
        {
            var todoList = await context.Lists.FindAsync(id);
            if (todoList == null) return false;

            context.Lists.Remove(todoList);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
