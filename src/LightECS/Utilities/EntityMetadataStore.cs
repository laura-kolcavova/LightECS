using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class EntityMetadataStore
    : IEntityMetadataStore
{
    private readonly Dictionary<uint, EntityMetadata> _entityMetadataByEntities;

    private readonly object _lock = new();

    public EntityMetadataStore()
    {
        _entityMetadataByEntities = new Dictionary<uint, EntityMetadata>();
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

    public void Set(
        Entity entity,
        EntityMetadata entityMetadata)
    {
        lock (_lock)
        {
            _entityMetadataByEntities[entity.Id] = entityMetadata;
        }
    }

    public void Unset(
        Entity entity)
    {
        lock (_lock)
        {
            _entityMetadataByEntities.Remove(
                entity.Id);
        }
    }
}
