using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentFlagsTests
{
    [Fact]
    public void Constructor_Default_ShouldInitializeWithZeroBits()
    {
        // Act
        var flags = new ComponentFlags();

        // Assert
        Assert.Equal(0ul, flags.Bits);
    }

    [Fact]
    public void Constructor_WithBits_ShouldInitializeWithGivenBits()
    {
        // Arrange
        ulong initialBits = 0b101010;

        // Act
        var flags = new ComponentFlags(initialBits);

        // Assert
        Assert.Equal(initialBits, flags.Bits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    public void SetTrueBitAtIndex_ShouldSetBitCorrectly(
        byte index)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var updatedFlags = flags.SetTrueBitAtIndex(index);

        // Assert
        var expectedBits = 1ul << index;
        Assert.Equal(expectedBits, updatedFlags.Bits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    public void SetFalseBitAtIndex_ShouldUnsetBitCorrectly(
        byte index)
    {
        // Arrange
        var initialBits = ulong.MaxValue;

        var flags = new ComponentFlags(initialBits);

        // Act
        var updatedFlags = flags.SetFalseBitAtIndex(index);

        // Assert
        var expectedBits = initialBits & ~(1ul << index);
        Assert.Equal(expectedBits, updatedFlags.Bits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(63)]
    public void Indexer_ShouldReturnTrue_WhenBitIsSet(byte index)
    {
        // Arrange
        var bits = 1ul << index;
        var flags = new ComponentFlags(bits);

        // Act
        var isSet = flags[index];

        // Assert
        Assert.True(isSet);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(63)]
    public void Indexer_ShouldReturnFalse_WhenBitIsNotSet(byte index)
    {
        // Arrange
        var flags = new ComponentFlags(0);

        // Act
        var isSet = flags[index];

        // Assert
        Assert.False(isSet);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void SetTrueBitAtIndex_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var act = () => { flags.SetTrueBitAtIndex(invalidIndex); };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void SetFalseBitAtIndex_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags(ulong.MaxValue);

        // Act
        var act = () => { flags.SetFalseBitAtIndex(invalidIndex); };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void Indexer_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var act = () => { var _ = flags[invalidIndex]; };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
