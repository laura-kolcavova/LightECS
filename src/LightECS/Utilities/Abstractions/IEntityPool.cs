namespace LightECS.Utilities.Abstractions;

internal interface IEntityPool
{
    public int Count { get; }

    public Entity Get();

    public void Return(
        Entity value);

    public bool Contains(
        Entity value);
}
