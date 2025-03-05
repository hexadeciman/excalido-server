using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class GenericRepository<TEntity>(DbContext context)
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task ReorderEntitiesAsync(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, int>> orderBySelector,
        Action<TEntity, int> setOrderIndex,
        object ownerId, int newIndexInput)
    {
        var entities = await _dbSet
            .Where(e => EF.Property<object>(e, "OwnerId").Equals(ownerId))
            .OrderBy(orderBySelector)
            .ToListAsync();

        var entity = await _dbSet.FirstOrDefaultAsync(filter);
        if (entity == null)
            throw new Exception($"{typeof(TEntity).Name} not found");

        var oldIndex = entities.FindIndex(e => e.Equals(entity));
        if (oldIndex == -1)
            throw new Exception($"{typeof(TEntity).Name} not found in ordering");

        int newIndex = Math.Clamp(newIndexInput, 0, entities.Count - 1);

        entities.RemoveAt(oldIndex);
        entities.Insert(newIndex, entity);

        for (int i = 0; i < entities.Count; i++)
        {
            setOrderIndex(entities[i], i);
            _dbSet.Update(entities[i]);
        }
    }
}

