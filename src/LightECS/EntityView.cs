using LightECS.Abstractions;
using LightECS.Events;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;

namespace LightECS;

public sealed class EntityView :
    IEntityView,
    IDisposable
{
    public event EntityAddedEventHandler? EntityAdded;

    public event EntityRemovedEventHandler? EntityRemoved;

    private readonly IComponentFlagIndexRegistry _componentFlagIndexRegistry;

    private readonly IComponentStoreRegistry _componentStoreRegistry;

    private readonly IEntityMetadataStore _entityMetadataStore;

    private readonly ComponentFlags _componentFlags;

    private readonly IEnumerable<Entity> _entityQuery;

    private readonly List<Entity> _entities;

    private readonly object _lock;

    private bool _disposed;

    private bool _isActive;

    internal EntityView(
        IComponentFlagIndexRegistry componentFlagIndexRegistry,
        IComponentStoreRegistry componentStoreRegistry,
        IEntityMetadataStore entityMetadataStore,
        ComponentFlags componentFlags,
        IEnumerable<Entity> entityQuery)
    {
        _componentFlagIndexRegistry = componentFlagIndexRegistry;
        _componentStoreRegistry = componentStoreRegistry;
        _entityMetadataStore = entityMetadataStore;
        _componentFlags = componentFlags;
        _entityQuery = entityQuery;

        _entities = [];
        _lock = new object();
        _disposed = false;
        _isActive = false;
    }

    ~EntityView()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        if (!_isActive)
        {
            Activate();
        }

        return _entities.AsEnumerable();
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _entityMetadataStore.EntityMetadataSet -= HandleEntityMetadataSet;
            _entityMetadataStore.EntityMetadataUnset -= HandleEntityMetadataUnset;
        }

        _disposed = true;
    }

    private void Activate()
    {
        lock (_lock)
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
        }

        _entities.AddRange(_entityQuery.AsEnumerable());

        _entityMetadataStore.EntityMetadataSet += HandleEntityMetadataSet;
        _entityMetadataStore.EntityMetadataUnset += HandleEntityMetadataUnset;
    }

    private void HandleEntityMetadataSet(
        Entity entity,
        EntityMetadata entityMetadata)
    {
        if (_componentFlags.ContainsAll(entityMetadata.ComponentFlags))
        {
            // todo
        }
    }

    private void HandleEntityMetadataUnset(
        Entity entity)
    {
    }
}
