using LightECS.Abstractions;
using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentStoreRegistryTests
{
    public class TestComponentA : IComponent
    {
        public int Value { get; set; }
    }

    public class TestComponentB : IComponent
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void GetOrCreate_ShouldCreateNewStore_WhenNotExists()
    {
        // Arrange
        var sut = new ComponentStoreRegistry(10, 10);

        // Act
        var store = sut.GetOrCreate<TestComponentA>(out var created);

        // Assert
        Assert.NotNull(store);
        Assert.Equal(0, store.Count);
        Assert.True(created);
    }

    [Fact]
    public void GetOrCreate_ShouldReturnStore_WhenExists()
    {
        // Arrange
        var sut = new ComponentStoreRegistry(10, 10);

        var expectedStore = sut.GetOrCreate<TestComponentA>(
          out var _);

        // Act
        var retrievedStore = sut.GetOrCreate<TestComponentA>(
            out var created);

        // Assert
        Assert.Same(expectedStore, retrievedStore);
        Assert.False(created);
    }

    [Fact]
    public void GetStore_ShouldReturnStore_WhenExists()
    {
        // Arrange
        var sut = new ComponentStoreRegistry(10, 10);

        var expectedStore = sut.GetOrCreate<TestComponentA>(
            out var _);

        // Act
        var retrievedStore = sut.Get<TestComponentA>();

        // Assert
        Assert.Same(expectedStore, retrievedStore);
    }

    [Fact]
    public void GetStore_ShouldThrow_WhenStoreNotExists()
    {
        // Arrange
        var provider = new ComponentStoreRegistry(10, 10);

        // Act
        var act = () => provider.Get<TestComponentA>();

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void GetAllStores_ShouldReturnAllCreatedStores()
    {
        // Arrange
        var provider = new ComponentStoreRegistry(10, 10);

        var storeA = provider.GetOrCreate<TestComponentA>(out var _);
        var storeB = provider.GetOrCreate<TestComponentB>(out var _);

        // Act
        var allStores = provider.GetAll().ToList();

        // Assert
        Assert.Equal(2, allStores.Count);
        Assert.Contains(storeA, allStores);
        Assert.Contains(storeB, allStores);
    }
}
