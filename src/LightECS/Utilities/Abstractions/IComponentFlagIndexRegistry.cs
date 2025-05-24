using LightECS.Abstractions;

namespace LightECS.Utilities.Abstractions;

internal interface IComponentFlagIndexRegistry
{
    public byte Get<TComponent>()
       where TComponent : IComponent;

    public byte GetOrCreate<TComponent>()
        where TComponent : IComponent;

    public byte Create<TComponent>()
        where TComponent : IComponent;
}
