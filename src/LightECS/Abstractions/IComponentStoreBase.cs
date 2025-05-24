namespace LightECS.Abstractions;

public interface IComponentStoreBase
{
    public int Count { get; }

    public bool Has(
        Entity entity);

    public void Unset(
        Entity entity);

    public void Clear();
}