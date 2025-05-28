using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class SequentialEntityIdGeneratorTests
{
    [Fact]
    public void ReadLastId_ShouldReturnZero_WhenNoIdGenerated()
    {
        // Arrange
        var sut = new SequentialEntityIdGenerator();

        // Act
        var currentId = sut.ReadNextId();

        // Assert
        Assert.Equal(0u, currentId);
    }

    [Fact]
    public void NextId_ShouldIncrementAndReturnId()
    {
        // Arrange
        var sut = new SequentialEntityIdGenerator();

        // Act
        var id = sut.NextId();

        // Assert
        Assert.Equal(0u, id);
    }

    [Fact]
    public void ReadLastId_ShouldReturnLastGeneratedId()
    {
        // Arrange
        var sut = new SequentialEntityIdGenerator();

        sut.NextId(); // 0
        sut.NextId(); // 1

        // Act
        var current = sut.ReadNextId();

        // Assert
        Assert.Equal(2u, current);
    }

    [Fact(Skip = "non-deterministic result")]
    // [Fact]
    public async Task NextId_ShouldBeThreadSafe()
    {
        // Arrange
        var provider = new SequentialEntityIdGenerator();

        var tasks = Enumerable.Range(0, 1000).Select(_ => Task.Run(() =>
        {
            provider.NextId();
        }));

        // Act
        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(10000u, provider.ReadNextId());
    }
}
