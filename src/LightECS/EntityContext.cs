using LightECS.Abstractions;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;

namespace LightECS;

public sealed class EntityContext :
    IEntityContext
{
    public const int InitialEntityStoreCapacity = 128;

    public const int InitialEntityPoolCapacity = 128;

    public const int InitialComponentCapacity = 64;

    public const int InitialComponentStoreCapacity = 128;

    private readonly EntityStore _entityStore;

    private readonly SequentialEntityIdGenerator _sequentialEntityIdGenerator;

    private readonly EntityPool _entityPool;

    private readonly EntityMetadataStore _entityMetadataStore;

    private readonly ComponentStoreRegistry _componentStoreRegistry;

    private readonly ComponentFlagIndexRegistry _componentFlagIndexRegistry;

    private readonly List<IComponentEventObserverBase> _componentEventObservers;

    private readonly ContextState _contextState;

    private bool _disposed;

    public EntityContext()
    {
        _entityStore = new EntityStore(
            InitialEntityStoreCapacity);

        _sequentialEntityIdGenerator = new SequentialEntityIdGenerator();

        _entityPool = new EntityPool(
            CreateNewEntity,
            InitialEntityPoolCapacity);

        _entityMetadataStore = new EntityMetadataStore(InitialEntityStoreCapacity);

        _componentStoreRegistry = new ComponentStoreRegistry(
            InitialComponentCapacity,
            InitialComponentStoreCapacity);

        _componentFlagIndexRegistry = new ComponentFlagIndexRegistry(
            InitialComponentCapacity);

        _componentEventObservers = new List<IComponentEventObserverBase>(InitialComponentCapacity);

        _contextState = new ContextState();
    }

    ~EntityContext()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
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

        _entityMetadataStore.Set(
            entity,
            () => EntityMetadata.Default(),
            metadata => EntityMetadata.Default());

        return entity;
    }

    public void DestroyEntity(
        Entity entity)
    {
        _entityStore.Remove(entity);

        _entityPool.Return(entity);

        foreach (var componentStoreBase in _componentStoreRegistry.GetAll())
        {
            componentStoreBase.Remove(entity);
        }

        _entityMetadataStore.Remove(entity);
    }

    public bool EntityExists(
        Entity entity)
    {
        return _entityStore.Contains(entity);
    }

    public IComponentStore<TComponent> UseStore<TComponent>()
       where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .GetOrCreate<TComponent>(out var created);

        if (created)
        {
            BindComponentStore(componentStore);
        }

        return componentStore;
    }

    public IEntityQuery UseQuery()
    {
        return new EntityQuery(
            _entityStore,
            _entityMetadataStore,
            _componentFlagIndexRegistry);
    }

    public void Set<TComponent>(
       Entity entity,
       TComponent component)
       where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .GetOrCreate<TComponent>(out var created);

        if (created)
        {
            BindComponentStore(componentStore);
        }

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

    public bool Contains<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        return componentStore.Contains(entity);
    }

    public void Remove<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = _componentStoreRegistry
            .Get<TComponent>();

        componentStore.Remove(entity);
    }

    private void Dispose(
        bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (var componentEventObserver in _componentEventObservers)
            {
                componentEventObserver.DetachStore();
            }
        }

        _disposed = true;
    }

    private Entity CreateNewEntity()
    {
        var entityId = _sequentialEntityIdGenerator.NextId();

        var entity = new Entity(entityId);

        return entity;
    }

    private void BindComponentStore<TComponent>(
        IComponentStore<TComponent> componentStore)
        where TComponent : IComponent
    {
        var componentEventObserver = new ComponentEventObserver<TComponent>(
            _componentFlagIndexRegistry,
            _entityMetadataStore);

        componentEventObserver.AttachStore(
            componentStore);

        _componentEventObservers.Add(componentEventObserver);
    }
}
