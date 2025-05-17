using LightECS.Abstractions;

namespace LightECS.Events;

public delegate void ComponentRemovedEventHandler<TComponent>(
    in Entity entity,
    in TComponent component)
    where TComponent : IComponent;
