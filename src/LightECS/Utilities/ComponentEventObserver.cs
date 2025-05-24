using LightECS.Abstractions;
using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class ComponentEventObserver<TComponent> :
    IComponentEventObserver<TComponent>
    where TComponent : IComponent
{
    private readonly IComponentFlagIndexRegistry _componentFlagIndexRegistry;

    private readonly IEntityMetadataStore _entityMetadataStore;

    private IComponentStore<TComponent>? _componentStore;

    public ComponentEventObserver(
        IComponentFlagIndexRegistry componentFlagIndexRegistry,
        IEntityMetadataStore entityMetadataStore)
    {
        _componentFlagIndexRegistry = componentFlagIndexRegistry;
        _entityMetadataStore = entityMetadataStore;
    }

    public void AttachStore(
        IComponentStore<TComponent> componentStore)
    {
        _componentStore = componentStore;

        _componentStore.ComponentAdded += HandleComponentAdded;
        _componentStore.ComponentRemoved += HandleComponentRemoved;
    }

    public void DetachStore()
    {
        if (_componentStore is null)
        {
            return;
        }

        _componentStore.ComponentAdded -= HandleComponentAdded;
        _componentStore.ComponentRemoved -= HandleComponentRemoved;

        _componentStore = null;
    }

    private void HandleComponentAdded(
        Entity entity,
        TComponent component)
    {
        var componentFlagIndex = _componentFlagIndexRegistry.GetOrCreate<TComponent>();

        _entityMetadataStore.Set(
            entity,
            () => new EntityMetadata
            {
                ComponentFlags = ComponentFlags.FromIndex(
                    componentFlagIndex)
            },
            (entityMetadata) => entityMetadata with
            {
                ComponentFlags = entityMetadata
                    .ComponentFlags
                    .SetFlagAtIndex(componentFlagIndex)
            });
    }

    private void HandleComponentRemoved(
        Entity entity,
        TComponent component)
    {
        var componentFlagIndex = _componentFlagIndexRegistry.GetOrCreate<TComponent>();

        _entityMetadataStore.Set(
            entity,
            () => EntityMetadata.Default(),
            (entityMetadata) => entityMetadata with
            {
                ComponentFlags = entityMetadata
                    .ComponentFlags
                    .UnsetFlagAtIndex(componentFlagIndex)
            });
    }
}
