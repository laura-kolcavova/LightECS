namespace LightECS.Utilities;

internal readonly struct ComponentFlags
{
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

    public bool this[byte index]
    {
        get
        {
            var mask = CreateMaskFromIndex(index);

            return (Bits & mask) == mask;
        }
    }

    public ComponentFlags SetTrueBitAtIndex(
        byte index)
    {
        var mask = CreateMaskFromIndex(index);

        var newBits = Bits | mask;

        return new ComponentFlags(newBits);
    }

    public ComponentFlags SetFalseBitAtIndex(
        byte index)
    {
        var mask = CreateMaskFromIndex(index);

        var newBits = Bits & ~mask;

        return new ComponentFlags(newBits);
    }

    private static ulong CreateMaskFromIndex(byte index)
    {
        ValidateIndex(index);

        return 1ul << index;
    }

    private static void ValidateIndex(byte index)
    {
        if (index < 0 || index >= 64)
        {
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"Index {index} must be between 0 and 63 (inclusive).");
        }
    }
}
