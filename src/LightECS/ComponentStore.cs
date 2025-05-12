using LightECS.Abstractions;

namespace LightECS;

public class ComponentStore<TComponent> :
    IComponentStore<TComponent>
    where TComponent : IComponent
{
    public const int DefaultInitialCapacity = 128;

    private readonly Dictionary<uint, TComponent> _componentsByEntities;

    public ComponentStore()
    {
        _componentsByEntities = new Dictionary<uint, TComponent>(DefaultInitialCapacity);
    }

    public int Count => _componentsByEntities.Count;

    public void Add(
        Entity entity,
        TComponent component)
    {
        if (_componentsByEntities.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException(
                $"Entity {entity.Id} already has a component of type {typeof(TComponent)}.");
        }

        _componentsByEntities[entity.Id] = component;
    }

    public IEnumerable<TComponent> AsEnumerable()
    {
        foreach (var component in _componentsByEntities.Values)
        {
            yield return component;
        }
    }

    public TComponent Get(
        Entity entity)
    {
        if (_componentsByEntities.TryGetValue(
            entity.Id,
            out var component))
        {
            return component;
        }

        throw new InvalidOperationException(
            $"Component of type {typeof(TComponent)} not found for entity {entity.Id}.");
    }

    public bool TryGet(
        Entity entity,
        out TComponent component)
    {
        return _componentsByEntities.TryGetValue(
            entity.Id, out component!);
    }

    public bool Has(
        Entity entity)
    {
        return _componentsByEntities.ContainsKey(entity.Id);
    }

    public void Remove(
        Entity entity)
    {
        _componentsByEntities.Remove(entity.Id);
    }

    public void RemoveAll()
    {
        _componentsByEntities.Clear();
    }

    public void Replace(
        Entity entity,
        TComponent component)
    {
        _componentsByEntities[entity.Id] = component;
    }
}
