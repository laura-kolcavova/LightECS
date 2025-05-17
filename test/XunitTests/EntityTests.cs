using LightECS;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class EntityTests
{
    [Fact]
    public void Constructor_SetsIdCorrectly()
    {
        // Arrange
        uint id = 123;

        // Act
        var entity = new Entity(id);

        // Assert
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Equals_ReturnsTrue_ForSameId()
    {
        // Arrange
        var e1 = new Entity(42);
        var e2 = new Entity(42);

        // Act
        var areEqual = e1.Equals(e2);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentId()
    {
        // Arrange
        var e1 = new Entity(1);
        var e2 = new Entity(2);

        // Act
        var areEqual = e1.Equals(e2);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void EqualsObject_ReturnsTrue_ForSameId()
    {
        // Arrange
        var e1 = new Entity(99);
        object e2 = new Entity(99);

        // Act
        var areEqual = e1.Equals(e2);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void EqualsObject_ReturnsFalse_ForDifferentType()
    {
        // Arrange
        var entity = new Entity(1);
        var obj = new object();

        // Act
        var areEqual = entity.Equals(obj);

        Assert.False(areEqual);
    }

    [Fact]
    public void EqualsObject_ReturnsFalse_ForNull()
    {
        // Arrange
        var entity = new Entity(1);

        // Act
        var areEqual = entity.Equals(null);

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void EqualsNullableEntity_ReturnsTrue_ForSameId()
    {
        // Arrange
        var e1 = new Entity(1);
        Entity? e2 = new Entity(1);

        // Act
        var areEqual = e1.Equals(e2);

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void EqualsNullableEntity_ReturnsFalse_ForNull()
    {
        // Arrange
        var e1 = new Entity(1);
        Entity? e2 = null;

        // Act
        var areEqual = e1.Equals(e2);

        // Assert
        Assert.False(areEqual);
    }


    [Fact]
    public void GetHashCode_ReturnsSameHash_ForSameId()
    {
        // Arrange
        var e1 = new Entity(7);
        var e2 = new Entity(7);

        // Act
        var hash1 = e1.GetHashCode();
        var hash2 = e2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void OperatorEquals_ReturnsTrue_WhenBothAreSame()
    {
        // Arrange
        Entity e1 = new Entity(10);
        Entity e2 = new Entity(10);

        // Act
        var areEqual = e1 == e2;

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void OperatorEquals_ReturnsFalse_WhenDifferent()
    {
        // Arrange
        Entity e1 = new Entity(10);
        Entity e2 = new Entity(20);

        // Act
        var areEqual = e1 == e2;

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void OperatorEquals_ReturnsTrue_WhenBothNull()
    {
        // Arrange
        Entity? e1 = null;
        Entity? e2 = null;

        // Act
        var areEqual = e1 == e2;

        // Assert
        Assert.True(areEqual);
    }

    [Fact]
    public void OperatorEquals_ReturnsFalse_WhenOneNull()
    {
        // Arrange
        Entity e1 = new Entity(5);
        Entity? e2 = null;

        // Act
        var areEqual = e1 == e2;

        // Assert
        Assert.False(areEqual);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsTrue_WhenDifferent()
    {
        // Arrange
        Entity e1 = new Entity(1);
        Entity e2 = new Entity(2);

        // Act
        var areNotEqual = e1 != e2;

        // Assert
        Assert.True(areNotEqual);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsFalse_WhenSame()
    {
        // Arrange
        Entity e1 = new Entity(1);
        Entity e2 = new Entity(1);

        // Act
        var areNotEqual = e1 != e2;

        // Assert
        Assert.False(areNotEqual);
    }
}
