using SharedCode.Models;

namespace SharedCode.Data;

/// <summary>
/// The command repository class. Implements the <see cref="ICommandRepository{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="ICommandRepository{TEntity}"/>
public abstract class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : Entity
{
    /// <inheritdoc/>
    public abstract int Add(TEntity entity);

    /// <inheritdoc/>
    public abstract IEnumerable<int> Add(IEnumerable<TEntity> entities);

    /// <inheritdoc/>
    int ICommandRepository.Add(Entity entity)
    {
        return entity is TEntity typed
            ? this.Add(typed)
            : throw new ArgumentException($"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
    }

    /// <inheritdoc/>
    IEnumerable<int> ICommandRepository.Add(IEnumerable<Entity> entities)
    {
        return entities is IEnumerable<TEntity> typed
            ? this.Add(typed)
            : throw new ArgumentException($"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
    }

    /// <inheritdoc/>
    public abstract IAddOrUpdateDescriptor AddOrUpdate(TEntity entity);

    /// <inheritdoc/>
    public abstract IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<TEntity> entities);

    /// <inheritdoc/>
    IAddOrUpdateDescriptor ICommandRepository.AddOrUpdate(Entity entity)
    {
        return entity is TEntity typed
            ? this.AddOrUpdate(typed)
            : throw new ArgumentException($"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
    }

    /// <inheritdoc/>
    IEnumerable<IAddOrUpdateDescriptor> ICommandRepository.AddOrUpdate(IEnumerable<Entity> entities)
    {
        return entities is IEnumerable<TEntity> typed
            ? this.AddOrUpdate(typed)
            : throw new ArgumentException($"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
    }

    /// <inheritdoc/>
    public abstract bool Remove(int id);

    /// <inheritdoc/>
    public abstract bool Remove(TEntity entity);

    /// <inheritdoc/>
    public abstract IDictionary<int, bool> Remove(IEnumerable<TEntity> entities);

    /// <inheritdoc/>
    bool ICommandRepository.Remove(Entity entity)
    {
        return entity is TEntity typed
            ? this.Remove(typed)
            : throw new ArgumentException($"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
    }

    /// <inheritdoc/>
    IDictionary<int, bool> ICommandRepository.Remove(IEnumerable<Entity> entities)
    {
        return entities is IEnumerable<TEntity> typed
            ? this.Remove(typed)
            : throw new ArgumentException($"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
    }

    /// <inheritdoc/>
    public abstract void Update(TEntity entity);

    /// <inheritdoc/>
    public abstract void Update(IEnumerable<TEntity> entities);

    /// <inheritdoc/>
    void ICommandRepository.Update(Entity entity)
    {
        if (entity is TEntity typed)
        {
            this.Update(typed);
        }
        else
        {
            throw new ArgumentException($"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
        }
    }

    /// <inheritdoc/>
    void ICommandRepository.Update(IEnumerable<Entity> entities)
    {
        if (entities is IEnumerable<TEntity> typed)
        {
            this.Update(typed);
        }
        else
        {
            throw new ArgumentException($"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
        }
    }
}
