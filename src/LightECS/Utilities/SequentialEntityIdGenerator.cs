using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class SequentialEntityIdGenerator :
    ISequentialEntityIdGenerator
{
    private readonly object _lock = new();

    private uint _nextEntityId = 0;

    public uint ReadNextId()
    {
        return _nextEntityId;
    }

    public uint NextId()
    {
        lock (_lock)
        {
            return _nextEntityId++;
        }
    }
}
