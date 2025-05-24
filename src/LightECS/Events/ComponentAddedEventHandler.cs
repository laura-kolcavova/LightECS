using LightECS.Abstractions;

namespace LightECS.Events;

public delegate void ComponentAddedEventHandler<TComponent>(
    Entity entity,
    TComponent component)
    where TComponent : IComponent;
