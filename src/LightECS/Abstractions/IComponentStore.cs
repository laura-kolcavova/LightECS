namespace LightECS.Abstractions;

public interface IComponentStore<TComponent>
    : IComponentStoreBase
    where TComponent : IComponent
{
    public void Set(
        Entity entity,
        TComponent component);

    public TComponent Get(
        Entity entity);

    public bool TryGet(
        Entity entity,
        out TComponent? component);

    public IEnumerable<TComponent> AsEnumerable();
}
