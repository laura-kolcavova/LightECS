using LightECS;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class EntityStoreTests
{
    [Fact]
    public void Count_ShouldBeZero_WhenEmpty()
    {
        // Arrange
        var store = new EntityStore();

        // Act
        var count = store.Count;

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Add_ShouldIncreaseCount()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);

        // Act
        store.Add(entity);

        // Assert
        Assert.Equal(1, store.Count);
        Assert.True(store.Contains(entity));
    }

    [Fact]
    public void Add_DuplicateEntity_ShouldThrow()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(42);

        store.Add(entity);

        // Act
        var act = () => store.Add(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenEntityExists()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(5);

        store.Add(entity);

        // Act
        var result = store.Contains(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenEntityNotExists()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(7);

        // Act
        var result = store.Contains(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Remove_ShouldReturnTrue_WhenEntityExists()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(10);

        store.Add(entity);

        // Act
        var removed = store.Remove(entity);

        // Assert
        Assert.True(removed);
        Assert.False(store.Contains(entity));
    }

    [Fact]
    public void Remove_ShouldReturnFalse_WhenEntityNotExists()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(999);

        // Act
        var removed = store.Remove(entity);

        // Assert
        Assert.False(removed);
    }

    [Fact]
    public void Clear_ShouldRemoveAllEntities()
    {
        // Arrange
        var store = new EntityStore();

        store.Add(new Entity(1));
        store.Add(new Entity(2));

        // Act
        store.Clear();

        // Assert
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void AsEnumerable_ShouldReturnAllEntities()
    {
        // Arrange
        var store = new EntityStore();

        var entity1 = new Entity(1);
        var entity2 = new Entity(2);

        store.Add(entity1);
        store.Add(entity2);

        // Act
        var entities = store.AsEnumerable();

        // Assert
        var list = new List<Entity>(entities);
        Assert.Equal(2, list.Count);
        Assert.Contains(entity1, list);
        Assert.Contains(entity2, list);
    }

    [Fact]
    public void Constructor_WithInitialCapacity_ShouldCreateInstance()
    {
        // Arrange & Act
        var store = new EntityStore(100);

        // Assert
        Assert.NotNull(store);
        Assert.Equal(0, store.Count);
    }
}
