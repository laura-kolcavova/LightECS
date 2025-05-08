namespace LightECS;

public sealed class Entity
{
    public uint Id { get; }

    public Entity(uint id)
    {
        Id = id;
    }
}
