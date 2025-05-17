using LightECS.Abstractions;

namespace LightECS.Events;

public delegate void ComponentAddedEventHandler<TComponent>(
    in Entity entity,
    in TComponent component)
    where TComponent : IComponent;
