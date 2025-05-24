using LightECS.Abstractions;
using LightECS.Events;
using System.Diagnostics.CodeAnalysis;

namespace LightECS;

public sealed class ComponentStore<TComponent> :
    IComponentStore<TComponent>
    where TComponent : IComponent
{
    private readonly Dictionary<uint, TComponent> _componentsByEntities;

    public event ComponentAddedEventHandler<TComponent>? ComponentAdded;

    public event ComponentUpdatedEventHandler<TComponent>? ComponentUpdated;

    public event ComponentRemovedEventHandler<TComponent>? ComponentRemoved;

    private readonly object _lock = new();

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
        lock (_lock)
        {
            if (_componentsByEntities.TryGetValue(
                entity.Id,
                out var existingComponent))
            {
                _componentsByEntities[entity.Id] = component;

                ComponentUpdated?.Invoke(
                    entity,
                    existingComponent,
                    component);

                return;
            }

            _componentsByEntities.Add(
                entity.Id,
                component);

            ComponentAdded?.Invoke(entity, component);
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
        [MaybeNullWhen(false)] out TComponent component)
    {
        return _componentsByEntities.TryGetValue(
            entity.Id,
            out component);
    }

    public bool Has(
        Entity entity)
    {
        return _componentsByEntities.ContainsKey(
            entity.Id);
    }

    public void Unset(
        Entity entity)
    {
        lock (_lock)
        {
            if (_componentsByEntities.Remove(
                entity.Id,
                out var component))
            {
                ComponentRemoved?.Invoke(entity, component);
            }
        }
    }

    public void Clear()
    {
        _componentsByEntities.Clear();
    }

    public IEnumerable<TComponent> AsEnumerable()
    {
        // TODO thread safe enumeration
        return _componentsByEntities
            .Values
            .AsEnumerable();
    }
}
