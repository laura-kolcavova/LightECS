using LightECS.Utilities.Abstractions;
using LightECS.Utilities.Events;

namespace LightECS.Utilities;

internal sealed class EntityMetadataStore
    : IEntityMetadataStore
{
    private readonly Dictionary<uint, EntityMetadata> _entityMetadataByEntities;

    private readonly object _lock = new();

    public event EntityMetadataSetEventHandler? EntityMetadataSet;

    public event EntityMetadataUnsetEventHandler? EntityMetadataUnset;

    public EntityMetadataStore(
        int initialCapacity)
    {
        _entityMetadataByEntities = new Dictionary<uint, EntityMetadata>(
            initialCapacity);
    }

    public EntityMetadataStore()
        : this(0)
    {
    }

    public EntityMetadata Get(
        Entity entity)
    {
        if (!_entityMetadataByEntities.TryGetValue(
            entity.Id,
            out var entityMetadata))
        {
            throw new InvalidOperationException(
                $"Entity metadata not found for entity {entity.Id}.");
        }

        return entityMetadata;
    }

    public bool TryGet(
        Entity entity,
        out EntityMetadata entityMetadata)
    {
        return _entityMetadataByEntities.TryGetValue(
            entity.Id,
            out entityMetadata);
    }

    public void Set(
        Entity entity,
        Func<EntityMetadata> addEntityMetadataFactory,
        Func<EntityMetadata, EntityMetadata> updateEntityMetadataFactory)
    {
        lock (_lock)
        {
            if (_entityMetadataByEntities.TryGetValue(
                entity.Id,
                out var existingEntityMetadata))
            {
                var updatedEntityMetadata = updateEntityMetadataFactory(
                    existingEntityMetadata);

                _entityMetadataByEntities[entity.Id] = updatedEntityMetadata;

                EntityMetadataSet?.Invoke(entity, updatedEntityMetadata);

                return;
            }


            var newEntityMetadata = addEntityMetadataFactory();

            _entityMetadataByEntities.Add(
                entity.Id,
                newEntityMetadata);

            EntityMetadataSet?.Invoke(entity, newEntityMetadata);
        }
    }

    public void Unset(
        Entity entity)
    {
        lock (_lock)
        {
            _entityMetadataByEntities.Remove(
                entity.Id);

            EntityMetadataUnset?.Invoke(entity);
        }
    }
}
