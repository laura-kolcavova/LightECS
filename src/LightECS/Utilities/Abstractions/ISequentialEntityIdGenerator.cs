namespace LightECS.Utilities.Abstractions;

internal interface ISequentialEntityIdGenerator
{
    public uint GetLastId();

    public uint NextId();
}
