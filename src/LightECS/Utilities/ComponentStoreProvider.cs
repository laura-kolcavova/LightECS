using LightECS.Abstractions;

namespace LightECS.Utilities;

internal sealed class ComponentStoreProvider
{
    private readonly Dictionary<Type, IComponentStoreBase> _componentStoresByType;

    private readonly int _initialComponentStoreCapacity;

    public ComponentStoreProvider(
        int initialComponentStoreCapacity)
    {
        _initialComponentStoreCapacity = initialComponentStoreCapacity;

        _componentStoresByType = [];
    }

    public IComponentStore<TComponent> GetOrCreateStore<TComponent>()
       where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        if (_componentStoresByType.TryGetValue(
            componentType,
            out var componentStoreBase))
        {
            return (ComponentStore<TComponent>)componentStoreBase;
        }

        var componentStore = new ComponentStore<TComponent>(
            _initialComponentStoreCapacity);

        _componentStoresByType[componentType] = componentStore;

        return componentStore;
    }

    public IComponentStore<TComponent> GetStore<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        if (_componentStoresByType.TryGetValue(
           componentType,
           out var componentStoreBase))
        {
            return (ComponentStore<TComponent>)componentStoreBase;
        }

        throw new InvalidOperationException(
            $"Component store for {typeof(TComponent)} does not exist.");
    }

    public IReadOnlyCollection<IComponentStoreBase> GetAllStores()
    {
        return _componentStoresByType
            .Values;
    }
}
