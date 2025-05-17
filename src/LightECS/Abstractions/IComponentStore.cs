using LightECS.Events;
using System.Diagnostics.CodeAnalysis;

namespace LightECS.Abstractions;

public interface IComponentStore<TComponent>
    : IComponentStoreBase
    where TComponent : IComponent
{
    public event ComponentAddedEventHandler<TComponent>? ComponentAdded;

    public event ComponentUpdatedEventHandler<TComponent>? ComponentUpdated;

    public event ComponentRemovedEventHandler<TComponent>? ComponentRemoved;

    public void Set(
        Entity entity,
        TComponent component);

    public TComponent Get(
        Entity entity);

    public bool TryGet(
        Entity entity,
        [MaybeNullWhen(false)] out TComponent component);

    public IEnumerable<TComponent> AsEnumerable();
}
