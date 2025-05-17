using LightECS.Abstractions;

namespace LightECS;

public class ContextState :
    IContextState
{
    private readonly Dictionary<string, object?> _state;

    public ContextState()
    {
        _state = [];
    }

    public void Clear()
    {
        _state.Clear();
    }

    public bool Contains(
        string key)
    {
        return _state.ContainsKey(key);
    }

    public TValue? Get<TValue>(
        string key)
    {
        if (_state.TryGetValue(
            key,
            out var value))
        {
            if (value is not TValue typedValue)
            {
                throw new InvalidCastException($"Stored value for key '{key}' is not of type {typeof(TValue).Name}.");
            }

            return typedValue;
        }

        return default;
    }

    public TValue GetRequired<TValue>(
        string key)
    {
        if (_state.TryGetValue(
            key,
            out var value))
        {
            if (value is not TValue typedValue)
            {
                throw new InvalidCastException($"Stored value for key '{key}' is not of type {typeof(TValue).Name}.");
            }

            return typedValue;
        }

        throw new KeyNotFoundException($"Key '{key}' not found in context state.");
    }

    public TValue Set<TValue>(
        string key,
        TValue value)
    {
        _state[key] = value;

        return value;
    }

    public void Unset(
        string key)
    {
        _state.Remove(key);
    }
}
