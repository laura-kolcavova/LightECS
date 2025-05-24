namespace LightECS.Utilities.Abstractions;

internal interface IEntityPool
{
    public int Count { get; }

    public Entity Get();

    public void Return(
        Entity entity);

    public bool Contains(
        Entity entity);
}
