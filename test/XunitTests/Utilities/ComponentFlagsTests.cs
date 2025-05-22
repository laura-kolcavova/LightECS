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
    [InlineData(5)]
    [InlineData(63)]
    public void HasFlagAtIndex_ShouldReturnTrue_WhenFlagIsSet(byte index)
    {
        // Arrange
        var bits = 1ul << index;
        var flags = new ComponentFlags(bits);

        // Act
        var isSet = flags.HasFlagAtIndex(index);

        // Assert
        Assert.True(isSet);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(63)]
    public void HasFlagAtIndex_ShouldReturnFalse_WhenFlagIsNotSet(byte index)
    {
        // Arrange
        var flags = new ComponentFlags(0);

        // Act
        var isSet = flags.HasFlagAtIndex(index);

        // Assert
        Assert.False(isSet);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void HasFlagAtIndex_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var act = () => { var _ = flags.HasFlagAtIndex(invalidIndex); };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    public void SetFlagAtIndex_ShouldSetBitCorrectly(
        byte index)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var updatedFlags = flags.SetFlagAtIndex(index);

        // Assert
        var expectedBits = 1ul << index;
        Assert.Equal(expectedBits, updatedFlags.Bits);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void SetFlagAtIndex_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags();

        // Act
        var act = () => { flags.SetFlagAtIndex(invalidIndex); };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    public void UnsetFlagAtIndex_ShouldUnsetBitCorrectly(
        byte index)
    {
        // Arrange
        var initialBits = ulong.MaxValue;

        var flags = new ComponentFlags(initialBits);

        // Act
        var updatedFlags = flags.UnsetFlagAtIndex(index);

        // Assert
        var expectedBits = initialBits & ~(1ul << index);
        Assert.Equal(expectedBits, updatedFlags.Bits);
    }

    [Theory]
    [InlineData(64)]
    [InlineData(100)]
    public void UnsetFlagAtIndex_ShouldThrow_WhenIndexOutOfRange(byte invalidIndex)
    {
        // Arrange
        var flags = new ComponentFlags(ulong.MaxValue);

        // Act
        var act = () => { flags.UnsetFlagAtIndex(invalidIndex); };

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(63)]
    public void Indexer_ShouldReturnTrue_WhenFlagIsSet(byte index)
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
    public void Indexer_ShouldReturnFalse_WhenFlagIsNotSet(byte index)
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
