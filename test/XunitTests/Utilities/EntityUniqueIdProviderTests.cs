using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class EntityUniqueIdProviderTests
{
    [Fact]
    public void GetCurrentId_ShouldReturnZero_WhenNoIdGenerated()
    {
        // Arrange
        var provider = new EntityUniqueIdProvider();

        // Act
        var currentId = provider.GetCurrentId();

        // Assert
        Assert.Equal(0u, currentId);
    }

    [Fact]
    public void GetNextId_ShouldIncrementAndReturnId()
    {
        // Arrange
        var provider = new EntityUniqueIdProvider();

        // Act
        var id1 = provider.GetNextId();
        var id2 = provider.GetNextId();
        var id3 = provider.GetNextId();

        // Assert
        Assert.Equal(1u, id1);
        Assert.Equal(2u, id2);
        Assert.Equal(3u, id3);
    }

    [Fact]
    public void GetCurrentId_ShouldReturnLastGeneratedId()
    {
        // Arrange
        var provider = new EntityUniqueIdProvider();
        provider.GetNextId(); // 1
        provider.GetNextId(); // 2

        // Act
        var current = provider.GetCurrentId();

        // Assert
        Assert.Equal(2u, current);
    }
}
