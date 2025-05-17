namespace LightECS.Abstractions;

public interface IComponentStoreBase
{
    public int Count { get; }

    public bool Has(
        Entity entity);

    public bool Remove(
        Entity entity);

    public void Clear();
}