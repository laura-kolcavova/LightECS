using LightECS.Abstractions;

namespace LightECS.Utilities.Abstractions;

internal interface IComponentFlagIndexRegistry
{
    public byte Get<TComponent>()
       where TComponent : IComponent;

    public byte GetOrRegister<TComponent>()
        where TComponent : IComponent;

    public byte Register<TComponent>()
        where TComponent : IComponent;
}
