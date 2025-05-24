using LightECS.Abstractions;

namespace LightECS.Events;

public delegate void ComponentRemovedEventHandler<TComponent>(
    Entity entity,
    TComponent component)
    where TComponent : IComponent;
