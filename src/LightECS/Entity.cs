namespace LightECS;

public sealed class Entity :
    IComparable,
    IComparable<Entity>
{
    public uint Id { get; }

    public Entity(uint id)
    {
        Id = id;
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

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        var entity = (Entity)obj;

        return Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        return CompareTo(obj as Entity);
    }

    public int CompareTo(Entity? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return Id.CompareTo(other.Id);
    }
}
