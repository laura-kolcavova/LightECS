namespace LightECS.Utilities.Abstractions;

internal interface IComponentFlagIndexRegistry
{
    public byte Get<TComponent>();

    public byte GetOrCreate<TComponent>();

    public byte Create<TComponent>();
}
