namespace LightECS.Utilities;

internal sealed class EntityUniqueIdProvider
{
    private uint _nextEntityId = 0;

    public uint GetCurrentId()
    {
        return _nextEntityId;
    }

    public uint GetNextId()
    {
        return ++_nextEntityId;
    }
}
