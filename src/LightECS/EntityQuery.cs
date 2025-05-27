using LightECS.Abstractions;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;

namespace LightECS;

public sealed class EntityQuery :
    IEntityQuery
{
    private readonly IEntityStore _entityStore;

    private readonly IEntityMetadataStore _entityMetadataStore;

    private readonly IComponentFlagIndexRegistry _componentFlagIndexRegistry;

    private readonly ComponentFlags _componentFlags;

    internal EntityQuery(
        IEntityStore entityStore,
        IEntityMetadataStore entityMetadataStore,
        IComponentFlagIndexRegistry componentFlagIndexRegistry,
        ComponentFlags componentFlags = default)
    {
        _entityStore = entityStore;
        _entityMetadataStore = entityMetadataStore;
        _componentFlagIndexRegistry = componentFlagIndexRegistry;
        _componentFlags = componentFlags;
    }

    private EntityQuery(
        EntityQuery entityQuery,
        ComponentFlags componentFlags)
        : this(
            entityQuery._entityStore,
            entityQuery._entityMetadataStore,
            entityQuery._componentFlagIndexRegistry,
            componentFlags)
    {
    }

    public IEntityQuery With<TComponent>()
        where TComponent : IComponent
    {
        var flagIndex = _componentFlagIndexRegistry.GetOrCreate<TComponent>();

        var newComponentFlags = _componentFlags.SetFlagAtIndex(
            flagIndex);

        return new EntityQuery(
            this,
            newComponentFlags);
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        foreach (var entity in _entityStore.AsEnumerable())
        {
            var entityMetadata = _entityMetadataStore.TryGet(entity, out var retrievedEntityMetadata)
                ? retrievedEntityMetadata
                : EntityMetadata.Default();

            var entityComponentFlags = entityMetadata.ComponentFlags;

            if (entityComponentFlags.HasNoFlags ||
                _componentFlags.HasNoFlags)
            {
                continue;
            }

            if ((entityComponentFlags & _componentFlags) == _componentFlags)
            {
                yield return entity;
            }
        }
    }
}
