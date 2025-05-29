using LightECS.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace LightECS;

public sealed class ContextState :
    IContextState
{
    private readonly Dictionary<string, object?> _state;

    private readonly object _lock = new();

    public ContextState()
    {
        _state = [];
    }

    public void Clear()
    {
        lock (_lock)
        {
            _state.Clear();
        }
    }

    public bool Contains(
        string key)
    {
        return _state.ContainsKey(key);
    }

    public TValue Get<TValue>(
        string key)
    {
        if (!_state.TryGetValue(
            key,
            out var value))
        {
            throw new KeyNotFoundException($"Key '{key}' not found in context state.");
        }

        if (value is not TValue typedValue)
        {
            throw new InvalidCastException($"Stored value for key '{key}' is not of type {typeof(TValue).Name}.");
        }

        return typedValue;
    }

    public bool TryGet<TValue>(
        string key,
        [MaybeNullWhen(false)] out TValue value)
    {
        if (!_state.TryGetValue(
            key,
            out var objectValue))
        {
            value = default;

            return false;
        }

        if (objectValue is not TValue typedValue)
        {
            throw new InvalidCastException($"Stored value for key '{key}' is not of type {typeof(TValue).Name}.");
        }

        value = typedValue;

        return true;
    }

    public void Set<TValue>(
        string key,
        TValue value)
    {
        lock (_lock)
        {
            _state[key] = value;
        }
    }

    public void Remove(
        string key)
    {
        lock (_lock)
        {
            _state.Remove(key);
        }
    }
}
