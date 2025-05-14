namespace LightECS.Abstractions;

public interface IContextState
{
    public TValue? Get<TValue>(
        string key);

    public TValue GetRequired<TValue>(
        string key);

    public TValue Set<TValue>(
        string key,
        TValue value);

    public void Unset(
        string key);

    public bool Contains(
        string key);

    public void Clear();
}
