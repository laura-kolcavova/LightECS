using LightECS.Abstractions;

namespace LightECS;

public class EntityStore :
    IEntityStore
{
    public const int DefaultInitialCapacity = 128;

    private readonly HashSet<Entity> _entities;

    public EntityStore()
    {
        _entities = new HashSet<Entity>(DefaultInitialCapacity);
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

    public void Remove(
        Entity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveAll()
    {
        _entities.Clear();
    }
}
