using LightECS.Abstractions;
using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class ComponentFlagIndexRegistry :
    IComponentFlagIndexRegistry
{
    private readonly Dictionary<Type, byte> _componentFlagIndexesByType;

    private readonly object _lock = new();

    private byte _nextFlagIndex = 0;

    public ComponentFlagIndexRegistry(
        int initialCapacity)
    {
        _componentFlagIndexesByType = new Dictionary<Type, byte>(
            initialCapacity);
    }

    public byte Get<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        if (!_componentFlagIndexesByType.TryGetValue(
            componentType,
            out var flagIndex))
        {
            throw new InvalidOperationException(
                $"Component type '{componentType.Name}' is not registered with a flag index.");
        }

        return flagIndex;
    }

    public byte GetOrCreate<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        lock (_lock)
        {
            if (_componentFlagIndexesByType.TryGetValue(
                componentType,
                out var flagIndex))
            {
                return flagIndex;
            }

            flagIndex = _nextFlagIndex++;

            _componentFlagIndexesByType.Add(
                componentType,
                flagIndex);

            return flagIndex;
        }
    }

    public byte Create<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        lock (_lock)
        {
            if (_componentFlagIndexesByType.ContainsKey(
                componentType))
            {
                throw new InvalidOperationException(
                    $"Component type '{componentType.Name}' is already registered with a flag index.");
            }

            var flagIndex = _nextFlagIndex++;

            _componentFlagIndexesByType.Add(
                componentType,
                flagIndex);

            return flagIndex;
        }
    }
}
