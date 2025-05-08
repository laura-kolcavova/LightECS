namespace LightECS.Abstractions;

public interface IComponentStoreBase
{
    public bool Has(
        Entity entity);

    public void Remove(
        Entity entity);

    public void RemoveAll();
}