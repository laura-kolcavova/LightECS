using LightECS.Abstractions;

namespace LightECS;

public class EntityStore :
    IEntityStore
{
    private readonly HashSet<Entity> _entities;

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
    }

    public bool Contains(Entity entity)
    {
        return _entities.Contains(entity);
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        foreach (var entity in _entities)
        {
            yield return entity;
        }
    }

    public bool Remove(
        Entity entity)
    {
        return _entities.Remove(entity);
    }

    public void Clear()
    {
        _entities.Clear();
    }
}
