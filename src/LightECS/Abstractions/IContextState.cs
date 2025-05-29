using System.Diagnostics.CodeAnalysis;

namespace LightECS.Abstractions;

public interface IContextState
{
    public TValue Get<TValue>(
        string key);

    public bool TryGet<TValue>(
        string key,
        [MaybeNullWhen(false)] out TValue value);

    public void Set<TValue>(
        string key,
        TValue value);

    public void Remove(
        string key);

    public bool Contains(
        string key);
}
