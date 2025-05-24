using LightECS;
using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class EntityMetadataStoreTests
{
    [Fact]
    public void Get_ShouldReturnMetadata_WhenEntityExists()
    {
        // Arrange
        var entity = new Entity(42);

        var metadata = EntityMetadata.Default();

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            addEntityMetadataFactory: () => metadata,
            updateEntityMetadataFactory: _ => metadata);

        // Act
        var result = sut.Get(entity);

        // Assert
        Assert.Same(metadata, result);
    }

    [Fact]
    public void Get_ShouldThrow_WhenEntityDoesNotExist()
    {
        // Arrange
        var entity = new Entity(1);

        var sut = new EntityMetadataStore();

        // Act
        var act = () => { sut.Get(entity); };

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Set_ShouldAddMetadata_WhenEntityDoesNotExist()
    {
        // Arrange
        var entity = new Entity(100);

        var metadata = EntityMetadata.Default();

        var sut = new EntityMetadataStore();

        // Act
        sut.Set(
            entity,
            addEntityMetadataFactory: () => metadata,
            updateEntityMetadataFactory: _ => throw new Exception("Should not be called"));

        // Assert
        var result = sut.Get(entity);
        Assert.Same(metadata, result);
    }

    [Fact]
    public void Set_ShouldUpdateMetadata_WhenEntityExists()
    {
        // Arrange
        var entity = new Entity(200);

        var initialMetadata = EntityMetadata.Default();

        var updatedMetadata = initialMetadata with
        {
            ComponentFlags = ComponentFlags.FromIndex(1)
        };

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            addEntityMetadataFactory: () => initialMetadata,
            updateEntityMetadataFactory: _ => initialMetadata);

        // Act
        sut.Set(
            entity,
            addEntityMetadataFactory: () => throw new Exception("Should not be called"),
            updateEntityMetadataFactory: _ => updatedMetadata);

        // Assert
        var result = sut.Get(entity);
        Assert.Same(updatedMetadata, result);
    }

    [Fact]
    public void Unset_ShouldRemoveMetadata_WhenEntityExists()
    {
        // Arrange
        var entity = new Entity(123);

        var metadata = EntityMetadata.Default();

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            addEntityMetadataFactory: () => metadata,
            updateEntityMetadataFactory: _ => metadata);

        // Act
        sut.Unset(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(() => sut.Get(entity));
    }

    [Fact]
    public void Unset_ShouldNotThrow_WhenEntityDoesNotExist()
    {
        // Arrange
        var entity = new Entity(404);

        var sut = new EntityMetadataStore();

        // Act
        var exception = Record.Exception(() => sut.Unset(entity));

        // Assert
        Assert.Null(exception);
    }
}
