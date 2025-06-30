using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class SequentialEntityIdGenerator :
    ISequentialEntityIdGenerator
{
    private readonly object _lock = new();

    private uint _currentEntityId = 0;

    public uint ReadId()
    {
        return _currentEntityId;
    }

    public uint NextId()
    {
        lock (_lock)
        {
            return ++_currentEntityId;
        }
    }
}
