using LightECS.Abstractions;
using LightECS.Events;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;

namespace LightECS;

public sealed class EntityView :
    IEntityView
{
    public event EntityAddedEventHandler? EntityAdded;

    public event EntityRemovedEventHandler? EntityRemoved;

    private readonly IEntityStore _entityStore;

    private readonly IComponentStoreRegistry _componentStoreRegistry;

    private readonly IEntityMetadataStore _entityMetadataStore;

    private readonly ComponentFlags _componentFlags;

    private readonly IEnumerable<Entity> _entityQuery;

    internal EntityView(
        IEntityStore entityStore,
        IComponentStoreRegistry componentStoreRegistry,
        IEntityMetadataStore entityMetadataStore,
        ComponentFlags componentFlags,
        IEnumerable<Entity> entityQuery)
    {
        _entityStore = entityStore;
        _componentStoreRegistry = componentStoreRegistry;
        _entityMetadataStore = entityMetadataStore;
        _componentFlags = componentFlags;
        _entityQuery = entityQuery;
    }

    public IEnumerable<Entity> AsEnumerable()
    {
        throw new NotImplementedException();
    }
}
