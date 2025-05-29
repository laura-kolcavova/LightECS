namespace LightECS.Abstractions;

public interface IComponentStoreBase
{
    public int Count { get; }

    public bool Contains(
        Entity entity);

    public void Remove(
        Entity entity);
}