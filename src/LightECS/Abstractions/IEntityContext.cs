using LightECS.Events;

namespace LightECS.Abstractions;

public interface IEntityContext :
    IDisposable
{
    public event EntityCreatedEventHandler? EntityCreated;

    public IContextState State { get; }

    public int EntitiesCount { get; }

    public IEntityStore UseEntityStore();

    public Entity CreateEntity();

    public void DestroyEntity(
        Entity entity);

    public bool EntityExists(
        Entity entity);

    public IComponentStore<TComponent> UseStore<TComponent>()
        where TComponent : IComponent;

    public void Set<TComponent>(
        Entity entity,
        TComponent component)
        where TComponent : IComponent;

    public TComponent Get<TComponent>(
        Entity entity)
        where TComponent : IComponent;

    public bool TryGet<TComponent>(
        Entity entity,
        out TComponent? component)
        where TComponent : IComponent;

    public int Count<TComponent>()
        where TComponent : IComponent;

    public bool Has<TComponent>(
        Entity entity)
        where TComponent : IComponent;

    public void Unset<TComponent>(
        Entity entity)
        where TComponent : IComponent;
}
