namespace LightECS.Abstractions;

public interface IEntityContext
{
    public int TotalCount { get; }

    public Entity Create();

    public void Destroy(
        Entity entity);

    public void DestroyAll();

    public bool Exists(
      Entity entity);

    public void Add<TComponent>(
        Entity entity,
        TComponent component)
        where TComponent : IComponent;

    public int Count<TComponent>()
        where TComponent : IComponent;

    public bool Has<TComponent>(
        Entity entity)
        where TComponent : IComponent;

    public TComponent Get<TComponent>(
        Entity entity)
        where TComponent : IComponent;

    public bool TryGet<TComponent>(
        Entity entity,
        out TComponent component)
        where TComponent : IComponent;

    public void Replace<TComponent>(
        Entity entity,
        TComponent component)
        where TComponent : IComponent;

    public void Remove<TComponent>(
        Entity entity)
        where TComponent : IComponent;

    public void RemoveAll<TComponent>()
        where TComponent : IComponent;

    public IComponentStore<TComponent> UseStore<TComponent>()
        where TComponent : IComponent;
}
