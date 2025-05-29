namespace LightECS.Abstractions;

public interface IEntityContext :
    IDisposable
{
    public IContextState State { get; }

    public int EntitiesCount { get; }

    public IEntityStore UseEntityStore();

    public Entity CreateEntity();

    public void DestroyEntity(
        Entity entity);

    public bool EntityExists(
        Entity entity);

    public IEntityQuery UseQuery();

    public IComponentStore<TComponent> UseStore<TComponent>();

    public void Set<TComponent>(
        Entity entity,
        TComponent component);

    public TComponent Get<TComponent>(
        Entity entity);

    public bool TryGet<TComponent>(
        Entity entity,
        out TComponent? component);

    public int Count<TComponent>();

    public bool Contains<TComponent>(
        Entity entity);

    public void Remove<TComponent>(
        Entity entity);
}
