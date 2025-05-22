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

    private readonly SequentialEntityIdGenerator _sequentialEntityIdGenerator;

    private readonly EntityPool _entityPool;

    private readonly EntityMetadataStore _entityMetadataStore;

    private readonly ComponentStoreRegistry _componentStoreRegistry;

    private readonly ContextState _contextState;

    public event EntityCreatedEventHandler? EntityCreated;

    public EntityContext()
    {
        _entityStore = new EntityStore(
            InitialEntityStoreCapacity);

        _sequentialEntityIdGenerator = new SequentialEntityIdGenerator();

        _entityPool = new EntityPool(
            CreateNewEntity,
            InitialEntityPoolCapacity);

        _entityMetadataStore = new EntityMetadataStore();

        _componentStoreRegistry = new ComponentStoreRegistry(
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
        foreach (var componentStoreBase in _componentStoreRegistry.GetAll())
        {
            componentStoreBase.Unset(entity);
        }

        _entityStore.Remove(entity);

        _entityPool.Return(entity);

        _entityMetadataStore.Unset(entity);
    }

    public bool EntityExists(
        Entity entity)
    {
        return _entityStore.Contains(entity);
    }

    public IComponentStore<TComponent> UseStore<TComponent>()
       where TComponent : IComponent
    {
        return _componentStoreRegistry
            .GetOrCreate<TComponent>();
    }

    public void Set<TComponent>(
       Entity entity,
       TComponent component)
       where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .GetOrCreate<TComponent>();

        componentStore.Set(
            entity,
            component);
    }

    public TComponent Get<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        return componentStore.Get(entity);
    }

    public bool TryGet<TComponent>(
        Entity entity,
        out TComponent? component)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        return componentStore.TryGet(
            entity,
            out component);
    }

    public int Count<TComponent>()
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        return componentStore.Count;
    }

    public bool Has<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        return componentStore.Has(entity);
    }

    public void Unset<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        componentStore.Unset(entity);
    }

    private Entity CreateNewEntity()
    {
        var entityId = _sequentialEntityIdGenerator.NextId();

        var entity = new Entity(entityId);

        return entity;
    }
}
