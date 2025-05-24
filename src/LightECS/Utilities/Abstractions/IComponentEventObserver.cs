using LightECS.Abstractions;

namespace LightECS.Utilities.Abstractions;

internal interface IComponentEventObserver<in TComponent> :
    IComponentEventObserverBase
    where TComponent : IComponent;
