namespace LightECS.Utilities;

internal readonly struct ComponentFlags :
    IEquatable<ComponentFlags>
{
    public const int BitsCount = 64;

    public ulong Bits { get; }

    public ComponentFlags()
    {
        Bits = 0;
    }

    public ComponentFlags(
        ulong bits)
    {
        Bits = bits;
    }

    public bool HasNoFlags => Bits == 0;

    public bool this[byte index]
    {
        get
        {
            return HasFlagAtIndex(index);
        }
    }

    public bool HasFlagAtIndex(
        byte index)
    {
        var mask = CreateBitsFromIndex(index);

        return (Bits & mask) == mask;
    }

    public ComponentFlags SetFlagAtIndex(
        byte index)
    {
        var mask = CreateBitsFromIndex(index);

        var newBits = Bits | mask;

        return new ComponentFlags(newBits);
    }

    public ComponentFlags UnsetFlagAtIndex(
        byte index)
    {
        var mask = CreateBitsFromIndex(index);

        var newBits = Bits & ~mask;

        return new ComponentFlags(newBits);
    }

    public bool HasFlags(ComponentFlags other)
    {
        return (this.Bits & other.Bits) == other.Bits;
    }

    public bool Equals(ComponentFlags other)
    {
        return other.Bits == Bits;
    }

    public override bool Equals(object? obj)
       => obj is ComponentFlags other && Equals(other);

    public override int GetHashCode()
       => Bits.GetHashCode();

    public static ComponentFlags FromIndex(byte index)
    {
        var bits = CreateBitsFromIndex(index);

        return new ComponentFlags(bits);
    }

    public static ComponentFlags None()
    {
        return new ComponentFlags();
    }

    private static ulong CreateBitsFromIndex(byte index)
    {
        ValidateIndex(index);

        return 1ul << index;
    }

    private static void ValidateIndex(byte index)
    {
        if (index < 0 || index >= BitsCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"Index {index} must be between 0 and 63 (inclusive).");
        }
    }
}
