using LightECS;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class EntityStoreTests
{
    [Fact]
    public void Add_NewEntity_IncreasesCount()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);

        // Act
        store.Add(entity);

        // Assert
        Assert.Equal(1, store.Count);
    }

    [Fact]
    public void Add_DuplicateEntity_ThrowsInvalidOperationException()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);

        store.Add(entity);

        // Act
        var act = () => store.Add(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Contains_ReturnsTrue_IfEntityExists()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);

        store.Add(entity);

        // Act
        var contains = store.Contains(entity);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_ReturnsFalse_IfEntityDoesNotExist()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);

        // Act
        var contains = store.Contains(entity);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void AsEnumerable_ReturnsAllEntities()
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
        Assert.Contains(entity1, entities);
        Assert.Contains(entity2, entities);
    }

    [Fact]
    public void Remove_DeletesEntity()
    {
        // Arrange
        var store = new EntityStore();

        var entity = new Entity(1);
        store.Add(entity);

        // Act
        store.Remove(entity);

        // Assert
        Assert.False(store.Contains(entity));
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void RemoveAll_ClearsStore()
    {
        // Arrange
        var store = new EntityStore();

        var entity1 = new Entity(1);
        var entity2 = new Entity(2);

        store.Add(entity1);
        store.Add(entity2);

        // Act
        store.RemoveAll();

        // Assert
        Assert.Equal(0, store.Count);
        Assert.Empty(store.AsEnumerable());
    }
}
