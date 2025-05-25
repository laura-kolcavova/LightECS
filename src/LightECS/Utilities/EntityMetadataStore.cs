using LightECS.Utilities.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace LightECS.Utilities;

internal sealed class EntityMetadataStore
    : IEntityMetadataStore
{
    private readonly Dictionary<uint, EntityMetadata> _entityMetadataByEntities;

    private readonly object _lock = new();

    public EntityMetadataStore()
    {
        _entityMetadataByEntities = [];
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
        [MaybeNullWhen(false)] out EntityMetadata entityMetadata)
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

                return;
            }

            var newEntityMetadata = addEntityMetadataFactory();

            _entityMetadataByEntities.Add(
                entity.Id,
                newEntityMetadata);
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
