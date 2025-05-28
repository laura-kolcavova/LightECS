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

    private readonly IEntityMetadataStore _entityMetadataStore;

    private readonly ComponentFlags _componentFlags;

    private readonly IEnumerable<Entity> _entityQuery;

    private readonly object _lock;

    private HashSet<Entity> _entities;

    private bool _disposed;

    private bool _isActive;

    internal EntityView(
        IEntityMetadataStore entityMetadataStore,
        ComponentFlags componentFlags,
        IEnumerable<Entity> entityQuery)
    {
        _entityMetadataStore = entityMetadataStore;
        _componentFlags = componentFlags;
        _entityQuery = entityQuery;

        _lock = new object();
        _entities = [];
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

        _entities = [.. _entityQuery];

        _entityMetadataStore.EntityMetadataSet += HandleEntityMetadataSet;
        _entityMetadataStore.EntityMetadataUnset += HandleEntityMetadataUnset;
    }

    private void HandleEntityMetadataSet(
        Entity entity,
        EntityMetadata entityMetadata)
    {
        if (_componentFlags.ContainsAll(entityMetadata.ComponentFlags) &&
            !_entities.Contains(entity))
        {
            _entities.Add(entity);

            EntityAdded?.Invoke(entity);
        }
    }

    private void HandleEntityMetadataUnset(
        Entity entity)
    {
        if (_entities.Remove(entity))
        {
            EntityRemoved?.Invoke(entity);
        }
    }
}
