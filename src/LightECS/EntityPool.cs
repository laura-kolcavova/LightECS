using LightECS.Extensions;

namespace LightECS;

public class EntityPool :
    PoolBase<Entity>
{
    public const int DefaultInitialCapacity = 128;

    private uint _nextEntityId = 1;

    public EntityPool()
        : base(DefaultInitialCapacity)
    {
    }

    protected override Entity Create()
    {
        var entity = new Entity(_nextEntityId);

        _nextEntityId++;

        return entity;
    }
}
