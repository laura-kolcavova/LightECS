using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentFlagIndexRegistryTests
{
    public class TestComponentA
    {
        public int Value { get; set; }
    }

    public class TestComponentB
    {
        public int Value { get; set; }
    }

    [Fact]
    public void Create_ShouldAssignNewFlagIndex()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        // Act
        var index = sut.Create<TestComponentA>();

        // Assert
        Assert.Equal(0, index);
    }

    [Fact]
    public void Create_ShouldThrow_WhenComponentTypeAlreadyRegistered()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        sut.Create<TestComponentA>();

        // Act
        var act = () => { sut.Create<TestComponentA>(); };

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Get_ShouldReturnCorrectIndex_WhenComponentIsRegistered()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        var expectedIndex = sut.Create<TestComponentA>();

        // Act
        var actualIndex = sut.Get<TestComponentA>();

        // Assert
        Assert.Equal(expectedIndex, actualIndex);
    }

    [Fact]
    public void Get_ShouldThrow_WhenComponentIsNotRegistered()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        // Act & Assert
        var act = () => { sut.Get<TestComponentA>(); };

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void GetOrCreate_ShouldReturnExistingIndex_IfAlreadyRegistered()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        var expectedIndex = sut.Create<TestComponentA>();

        // Act
        var actualIndex = sut.GetOrCreate<TestComponentA>();

        // Assert
        Assert.Equal(expectedIndex, actualIndex);
    }

    [Fact]
    public void GetOrCreate_ShouldCreateAndReturnNewIndex_IfNotRegistered()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        // Act
        var index = sut.GetOrCreate<TestComponentA>();

        // Assert
        Assert.Equal(0, index);
    }

    [Fact]
    public void Create_ShouldIncrementFlagIndexForEachComponent()
    {
        // Arrange
        var sut = new ComponentFlagIndexRegistry(initialCapacity: 10);

        // Act
        var index1 = sut.Create<TestComponentA>();
        var index2 = sut.Create<TestComponentB>();

        // Assert
        Assert.Equal(0, index1);
        Assert.Equal(1, index2);
    }
}
