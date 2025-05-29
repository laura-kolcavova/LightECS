using LightECS.Abstractions;
using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class ComponentStoreRegistry :
    IComponentStoreRegistry
{
    private readonly Dictionary<Type, IComponentStoreBase> _componentStoresByType;

    private readonly int _initialComponentStoreCapacity;

    private readonly object _lock = new();

    public ComponentStoreRegistry(
        int initialComponentCapacity,
        int initialComponentStoreCapacity)
    {
        _componentStoresByType = new Dictionary<Type, IComponentStoreBase>(
            initialComponentCapacity);

        _initialComponentStoreCapacity = initialComponentStoreCapacity;
    }

    public IComponentStore<TComponent> Get<TComponent>()
    {
        var componentType = typeof(TComponent);

        if (!_componentStoresByType.TryGetValue(
            componentType,
            out var componentStoreBase))
        {
            throw new InvalidOperationException(
                $"Component store for {typeof(TComponent)} does not exist.");
        }

        return (ComponentStore<TComponent>)componentStoreBase;
    }

    public IComponentStore<TComponent> GetOrCreate<TComponent>(
        out bool created)
    {
        var componentType = typeof(TComponent);

        lock (_lock)
        {
            if (_componentStoresByType.TryGetValue(
                componentType,
                out var componentStoreBase))
            {
                created = false;

                return (ComponentStore<TComponent>)componentStoreBase; ;
            }

            var componentStore = new ComponentStore<TComponent>(
                _initialComponentStoreCapacity);

            _componentStoresByType[componentType] = componentStore;

            created = true;

            return componentStore;
        }
    }

    public IReadOnlyCollection<IComponentStoreBase> GetAll()
    {
        return _componentStoresByType
            .Values;
    }
}
