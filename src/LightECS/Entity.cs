namespace LightECS;

public readonly struct Entity :
    IEquatable<Entity>
{
    public uint Id { get; }

    public Entity(uint id)
    {
        Id = id;
    }

    public bool Equals(
        Entity other)
    {
        return other.Id == Id;
    }

    public override bool Equals(
        object? obj)
    {
        if (obj is not Entity entity)
        {
            return false;
        }

        return Equals(entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(
       Entity? first,
       Entity? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(
        Entity? first,
        Entity? second)
    {
        return !(first == second);
    }
}
