using LightECS.Abstractions;
using LightECS.Events;

namespace LightECS;

public class EntityStore :
    IEntityStore
{
    private readonly HashSet<Entity> _entities;

    public event EntityAddedEventHandler? EntityAdded;

    public event EntityRemovedEventHandler? EntityRemoved;

    private readonly object _lock = new();

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
        lock (_lock)
        {
            if (!_entities.Add(entity))
            {
                throw new InvalidOperationException(
                    $"Entity {entity.Id} is already present in the store.");
            }

            EntityAdded?.Invoke(entity);
        }
    }

    public bool Contains(
        Entity entity)
    {
        return _entities.Contains(entity);
    }

    public bool Remove(
        Entity entity)
    {
        lock (_lock)
        {
            if (!_entities.Remove(entity))
            {
                return false;
            }

            EntityRemoved?.Invoke(entity);

            return true;
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            var oldEntities = _entities.ToList();

            _entities.Clear();

            foreach (var entity in oldEntities)
            {
                EntityRemoved?.Invoke(entity);
            }
        }
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        // TODO: thread safe enumeration
        return _entities.AsEnumerable();
    }
}
