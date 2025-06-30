namespace LightECS.Utilities.Abstractions;

internal interface ISequentialEntityIdGenerator
{
    public uint ReadId();

    public uint NextId();
}
