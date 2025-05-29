namespace LightECS.Events;

public delegate void ComponentUpdatedEventHandler<TComponent>(
    Entity entity,
    TComponent oldComponent,
    TComponent newComponent);
