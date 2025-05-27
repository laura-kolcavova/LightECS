namespace LightECS.Utilities.Abstractions;

internal interface ISequentialEntityIdGenerator
{
    public uint ReadNextId();

    public uint NextId();
}
