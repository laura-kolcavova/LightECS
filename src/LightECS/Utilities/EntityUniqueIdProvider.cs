namespace LightECS.Utilities;

internal sealed class EntityUniqueIdProvider
{
    private readonly object _lock = new();

    private uint _nextEntityId = 0;

    public uint GetCurrentId()
    {
        return _nextEntityId;
    }

    public uint GetNextId()
    {
        lock (_lock)
        {
            return ++_nextEntityId;
        }
    }
}
