using LightECS.Abstractions;
using LightECS.Events;
using LightECS.Utilities;

namespace LightECS;

public class EntityContext :
    IEntityContext
{
    public const int InitialEntityStoreCapacity = 128;

    public const int InitialEntityPoolCapacity = 128;

    public const int InitialComponentStoreCapacity = 128;

    private readonly EntityStore _entityStore;

    private readonly Pool<Entity> _entityPool;

    private readonly EntityUniqueIdProvider _entityUniqueIdProvider;

    private readonly ComponentStoreProvider _componentStoreProvider;

    private readonly ContextState _contextState;

    public event EntityCreatedEventHandler? EntityCreated;

    public EntityContext()
    {
        _entityStore = new EntityStore(
            InitialEntityStoreCapacity);

        _entityPool = new Pool<Entity>(
            CreateNewEntity,
            InitialEntityPoolCapacity);

        _entityUniqueIdProvider = new EntityUniqueIdProvider();

        _componentStoreProvider = new ComponentStoreProvider(
            InitialComponentStoreCapacity);

        _contextState = new ContextState();
    }

    public IContextState State => _contextState;

    public int EntitiesCount => _entityStore.Count;

    public IEntityStore UseEntityStore()
    {
        return _entityStore;
    }

    public Entity CreateEntity()
    {
        var entity = _entityPool.Get();

        _entityStore.Add(entity);

        EntityCreated?.Invoke(entity);

        return entity;
    }

    public void DestroyEntity(
        Entity entity)
    {
        foreach (var componentStoreBase in _componentStoreProvider.GetAllStores())
        {
            componentStoreBase.Remove(entity);
        }

        _entityStore.Remove(entity);

        _entityPool.Return(entity);
    }

    public bool EntityExists(
        Entity entity)
    {
        return _entityStore.Contains(entity);
    }

    public IComponentStore<TComponent> UseStore<TComponent>()
       where TComponent : IComponent
    {
        return _componentStoreProvider
            .GetOrCreateStore<TComponent>();
    }

    public void Set<TComponent>(
       Entity entity,
       TComponent component)
       where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetOrCreateStore<TComponent>();

        componentStore.Set(
            entity,
            component);
    }

    public TComponent Get<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetStore<TComponent>();

        return componentStore.Get(entity);
    }

    public bool TryGet<TComponent>(
        Entity entity,
        out TComponent? component)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetStore<TComponent>();

        return componentStore.TryGet(
            entity,
            out component);
    }

    public int Count<TComponent>()
        where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetStore<TComponent>();

        return componentStore.Count;
    }

    public bool Has<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetStore<TComponent>();

        return componentStore.Has(entity);
    }

    public void Remove<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreProvider
            .GetStore<TComponent>();

        componentStore.Remove(entity);
    }

    private Entity CreateNewEntity()
    {
        var entityId = _entityUniqueIdProvider.GetNextId();

        var entity = new Entity(entityId);

        return entity;
    }
}
