using LightECS.Abstractions;

namespace LightECS.Events;

public delegate void ComponentUpdatedEventHandler<TComponent>(
    in Entity entity,
    in TComponent oldComponent,
    in TComponent newComponent)
    where TComponent : IComponent;
