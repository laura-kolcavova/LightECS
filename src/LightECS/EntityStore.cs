using LightECS.Abstractions;
using LightECS.Events;

namespace LightECS;

public class EntityStore :
    IEntityStore
{
    private readonly HashSet<Entity> _entities;

    public event EntityAddedEventHandler? EntityAdded;

    public event EntityRemovedEventHandler? EntityRemoved;

    public EntityStore()
    {
        _entities = [];
    }

    public EntityStore(
        int initialCapacity)
    {
        _entities = new HashSet<Entity>(
            initialCapacity);
    }

    public int Count => _entities.Count;

    public void Add(
        Entity entity)
    {
        if (!_entities.Add(entity))
        {
            throw new InvalidOperationException(
                $"Entity {entity.Id} is already present in the store.");
        }

        EntityAdded?.Invoke(entity);
    }

    public bool Contains(
        Entity entity)
    {
        return _entities.Contains(entity);
    }

    public bool Remove(
        Entity entity)
    {
        if (!_entities.Remove(entity))
        {
            return false;
        }

        EntityRemoved?.Invoke(entity);

        return true;
    }

    public void Clear()
    {
        _entities.Clear();
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        return _entities.AsEnumerable();
    }
}
