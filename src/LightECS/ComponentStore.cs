using LightECS.Abstractions;

namespace LightECS;

public class ComponentStore<TComponent> :
    IComponentStore<TComponent>
    where TComponent : IComponent
{
    private readonly Dictionary<uint, TComponent> _componentsByEntities;

    public ComponentStore()
    {
        _componentsByEntities = [];
    }

    public ComponentStore(
        int initialCapacity)
    {
        _componentsByEntities = new Dictionary<uint, TComponent>(
            initialCapacity);
    }

    public int Count => _componentsByEntities.Count;

    public void Set(
        Entity entity,
        TComponent component)
    {
        _componentsByEntities[entity.Id] = component;
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
        out TComponent? component)
    {
        return _componentsByEntities.TryGetValue(
            entity.Id, out component!);
    }

    public bool Has(
        Entity entity)
    {
        return _componentsByEntities.ContainsKey(entity.Id);
    }

    public bool Remove(
        Entity entity)
    {
        return _componentsByEntities.Remove(entity.Id);
    }

    public void Clear()
    {
        _componentsByEntities.Clear();
    }

    public IEnumerable<TComponent> AsEnumerable()
    {
        foreach (var component in _componentsByEntities.Values)
        {
            yield return component;
        }
    }
}
