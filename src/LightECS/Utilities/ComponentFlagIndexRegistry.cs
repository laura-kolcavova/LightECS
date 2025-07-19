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
    {
        var componentType = typeof(TComponent);

        return Get(componentType);
    }

    public byte GetOrCreate<TComponent>()
    {
        var componentType = typeof(TComponent);

        return GetOrCreate(componentType);
    }

    public byte Create<TComponent>()
    {
        var componentType = typeof(TComponent);

        return Create(componentType);
    }

    private byte Get(Type componentType)
    {
        if (!_componentFlagIndexesByType.TryGetValue(
            componentType,
            out var flagIndex))
        {
            throw new InvalidOperationException(
                $"Component type '{componentType.Name}' is not registered with a flag index.");
        }

        return flagIndex;
    }

    private byte GetOrCreate(Type componentType)
    {
        lock (_lock)
        {
            if (_componentFlagIndexesByType.TryGetValue(
                componentType,
                out var flagIndex))
            {
                return flagIndex;
            }

            flagIndex = NextFlagIndex();

            _componentFlagIndexesByType.Add(
                componentType,
                flagIndex);

            return flagIndex;
        }
    }

    private byte Create(Type componentType)
    {
        lock (_lock)
        {
            if (_componentFlagIndexesByType.ContainsKey(
                componentType))
            {
                throw new InvalidOperationException(
                    $"Component type '{componentType.Name}' is already registered with a flag index.");
            }

            var flagIndex = NextFlagIndex();

            _componentFlagIndexesByType.Add(
                componentType,
                flagIndex);

            return flagIndex;
        }
    }

    private byte NextFlagIndex()
    {
        ValidateNextFlagIndex();

        return _nextFlagIndex++;
    }

    private void ValidateNextFlagIndex()
    {
        if (_nextFlagIndex >= ComponentFlags.BitsCount)
        {
            throw new InvalidOperationException(
                $"Cannot register more than {ComponentFlags.BitsCount} component types in a single component flag registry.");
        }
    }
}
