using LightECS.Abstractions;
using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentStoreProviderTests
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
    public void GetOrCreateStore_ShouldCreateNewStore_WhenNotExists()
    {
        // Arrange
        var provider = new ComponentStoreProvider(initialComponentStoreCapacity: 10);

        // Act
        var store = provider.GetOrCreateStore<TestComponentA>();

        // Assert
        Assert.NotNull(store);
        Assert.Equal(0, store.Count);
    }

    [Fact]
    public void GetStore_ShouldReturnStore_WhenExists()
    {
        // Arrange
        var provider = new ComponentStoreProvider(10);
        var expectedStore = provider.GetOrCreateStore<TestComponentA>();

        // Act
        var retrievedStore = provider.GetStore<TestComponentA>();

        // Assert
        Assert.Same(expectedStore, retrievedStore);
    }

    [Fact]
    public void GetStore_ShouldThrow_WhenStoreNotExists()
    {
        // Arrange
        var provider = new ComponentStoreProvider(10);

        // Act
        var act = () => provider.GetStore<TestComponentA>();

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void GetAllStores_ShouldReturnAllCreatedStores()
    {
        // Arrange
        var provider = new ComponentStoreProvider(10);
        var storeA = provider.GetOrCreateStore<TestComponentA>();
        var storeB = provider.GetOrCreateStore<TestComponentB>();

        // Act
        var allStores = provider.GetAllStores().ToList();

        // Assert
        Assert.Equal(2, allStores.Count);
        Assert.Contains(storeA, allStores);
        Assert.Contains(storeB, allStores);
    }
}
