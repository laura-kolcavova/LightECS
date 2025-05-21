using LightECS;
using LightECS.Abstractions;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentStoreTests
{
    public class TestComponent : IComponent
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void Count_ShouldBeZero_WhenEmpty()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();

        // Act
        var count = store.Count;

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Set_ShouldAddComponent()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(1);
        var component = new TestComponent { Name = "Test" };

        // Act
        store.Set(entity, component);

        // Assert
        Assert.Equal(1, store.Count);
        Assert.True(store.Has(entity));
    }

    [Fact]
    public void Get_ShouldReturnComponent_WhenExists()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(1);
        var component = new TestComponent { Name = "Test" };
        store.Set(entity, component);

        // Act
        var result = store.Get(entity);

        // Assert
        Assert.Equal(component, result);
    }

    [Fact]
    public void Get_ShouldThrow_WhenComponentNotFound()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();

        var entity = new Entity(1);

        // Act
        var act = () => store.Get(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void TryGet_ShouldReturnTrue_WhenComponentExists()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(1);
        var component = new TestComponent { Name = "Found" };
        store.Set(entity, component);

        // Act
        var result = store.TryGet(entity, out var retrieved);

        // Assert
        Assert.True(result);
        Assert.Equal(component, retrieved);
    }

    [Fact]
    public void TryGet_ShouldReturnFalse_WhenComponentMissing()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(2);

        // Act
        var result = store.TryGet(entity, out var retrieved);

        // Assert
        Assert.False(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void Has_ShouldReturnTrue_WhenComponentExists()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(3);
        store.Set(entity, new TestComponent());

        // Act
        var result = store.Has(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Has_ShouldReturnFalse_WhenComponentNotExists()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(99);

        // Act
        var result = store.Has(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Remove_ShouldReturnTrue_WhenComponentExists()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(1);
        store.Set(entity, new TestComponent());

        // Act
        var removed = store.Remove(entity);

        // Assert
        Assert.True(removed);
        Assert.False(store.Has(entity));
    }

    [Fact]
    public void Remove_ShouldReturnFalse_WhenComponentDoesNotExist()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var entity = new Entity(404);

        // Act
        var result = store.Remove(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Clear_ShouldRemoveAllComponents()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        store.Set(new Entity(1), new TestComponent());
        store.Set(new Entity(2), new TestComponent());

        // Act
        store.Clear();

        // Assert
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void AsEnumerable_ShouldReturnAllComponents()
    {
        // Arrange
        var store = new ComponentStore<TestComponent>();
        var comp1 = new TestComponent { Name = "A" };
        var comp2 = new TestComponent { Name = "B" };
        store.Set(new Entity(1), comp1);
        store.Set(new Entity(2), comp2);

        // Act
        var components = store.AsEnumerable();

        // Assert
        var list = new List<TestComponent>(components);
        Assert.Equal(2, list.Count);
        Assert.Contains(comp1, list);
        Assert.Contains(comp2, list);
    }

    [Fact]
    public void Constructor_WithInitialCapacity_ShouldCreateInstance()
    {
        // Arrange & Act
        var store = new ComponentStore<TestComponent>(50);

        // Assert
        Assert.NotNull(store);
        Assert.Equal(0, store.Count);
    }
}
