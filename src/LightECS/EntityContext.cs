using LightECS.Abstractions;
using LightECS.Extensions;

namespace LightECS;

public class EntityContext :
    IEntityContext
{
    public const int EntityStoreInitialCapacity = 128;

    public const int EntityPoolInitialCapacity = 128;

    public const int ComponentStoreInitialCapacity = 128;

    private readonly EntityStore _entityStore;

    private readonly Pool<Entity> _entityPool;

    private readonly Dictionary<Type, IComponentStoreBase> _componentStoresByType;

    private readonly ContextState _contextState;

    private uint _nextEntityId = 1;

    public EntityContext()
    {
        _entityStore = new EntityStore(
            EntityStoreInitialCapacity);

        _entityPool = new Pool<Entity>(
            CreateNewEntity,
            EntityPoolInitialCapacity);

        _componentStoresByType = [];

        _contextState = new ContextState();
    }

    public int TotalCount => _entityStore.Count;

    public IContextState State => _contextState;

    /// <summary>
    /// Creates a new entity with no components.
    /// </summary>
    /// <returns>A newly created entity.</returns>
    public Entity Create()
    {
        var entity = _entityPool.Get();

        _entityStore.Add(entity);

        return entity;
    }

    /// <summary>
    /// Destroys an existing entity and removes all its components.
    /// </summary>
    /// <param name="entity">The entity to be destroyed.</param>
    public void Destroy(Entity entity)
    {
        foreach (var componentStoreBase in _componentStoresByType.Values)
        {
            componentStoreBase.Remove(entity);
        }

        _entityStore.Remove(entity);

        _entityPool.Return(entity);
    }

    public void DestroyAll()
    {
        foreach (var entity in _entityStore.AsEnumerable())
        {
            Destroy(entity);
        }
    }

    public bool Exists(Entity entity)
    {
        return _entityStore.Contains(entity);
    }

    public IEntityStore UseStore()
    {
        return _entityStore;
    }

    public void Add<TComponent>(
       Entity entity,
       TComponent component)
       where TComponent : IComponent
    {
        var componentStore = UseStore<TComponent>();

        componentStore.Add(
            entity,
            component);
    }

    public int Count<TComponent>()
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        return componentStore.Count;
    }

    public bool Has<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        return componentStore.Has(entity);
    }

    public TComponent Get<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        return componentStore.Get(entity);
    }

    public bool TryGet<TComponent>(
       Entity entity,
       out TComponent component)
       where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        return componentStore.TryGet(
            entity,
            out component);
    }

    public void Remove<TComponent>(
        Entity entity)
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        componentStore.Remove(entity);
    }

    public void RemoveAll<TComponent>()
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        componentStore.RemoveAll();
    }

    public void Replace<TComponent>(
        Entity entity,
        TComponent component)
        where TComponent : IComponent
    {
        var componentStore = GetStore<TComponent>();

        componentStore.Replace(
            entity,
            component);
    }

    public IComponentStore<TComponent> UseStore<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        if (_componentStoresByType.TryGetValue(
            componentType,
            out var componentStoreBase))
        {
            return (ComponentStore<TComponent>)componentStoreBase;
        }

        var componentStore = new ComponentStore<TComponent>(
            ComponentStoreInitialCapacity);

        _componentStoresByType[componentType] = componentStore;

        return componentStore;
    }

    private IComponentStore<TComponent> GetStore<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);

        if (_componentStoresByType.TryGetValue(
           componentType,
           out var componentStoreBase))
        {
            return (ComponentStore<TComponent>)componentStoreBase;
        }

        throw new InvalidOperationException(
            $"Component store for {typeof(TComponent)} does not exist.");
    }

    private Entity CreateNewEntity()
    {
        var entity = new Entity(_nextEntityId);

        _nextEntityId++;

        return entity;
    }
}
