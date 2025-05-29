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
        Assert.Equal(metadata, result);
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
    public void TryGet_ShouldReturnTrue_AndOutputValue_WhenKeyExistsWithCorrectType()
    {
        // Arrange
        var entity = new Entity(1);

        var expectedEntityMetadata = EntityMetadata.Default() with
        {
            ComponentFlags = ComponentFlags.FromIndex(1)
        };

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            addEntityMetadataFactory: () => expectedEntityMetadata,
            updateEntityMetadataFactory: _ => expectedEntityMetadata);

        // Act
        var success = sut.TryGet(entity, out var retrievedEntityMetadata);

        // Assert
        Assert.True(success);
        Assert.Equal(expectedEntityMetadata, retrievedEntityMetadata);
    }

    [Fact]
    public void TryGet_ShouldReturnFalse_AndDefaultOut_WhenKeyDoesNotExist()
    {
        // Arrange
        var entity = new Entity(1);

        var sut = new EntityMetadataStore();

        // Act
        var success = sut.TryGet(entity, out var retrievedEntityMetadata);

        // Assert
        Assert.False(success);
        Assert.Equal(default, retrievedEntityMetadata);
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
        Assert.Equal(metadata, result);
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
        Assert.Equal(updatedMetadata, result);
    }

    [Fact]
    public void Remove_ShouldRemoveMetadata_WhenEntityExists()
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
        sut.Remove(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(() => sut.Get(entity));
    }

    [Fact]
    public void Remove_ShouldNotThrow_WhenEntityDoesNotExist()
    {
        // Arrange
        var entity = new Entity(404);

        var sut = new EntityMetadataStore();

        // Act
        var exception = Record.Exception(() => sut.Remove(entity));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenEntityIsNotPresent()
    {
        // Arrange
        var entity = new Entity(1);

        var sut = new EntityMetadataStore();

        // Act
        var result = sut.Contains(entity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenEntityIsPresent()
    {
        // Arrange
        var entity = new Entity(42);

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            () => EntityMetadata.Default(),
            existing => existing);

        // Act
        var result = sut.Contains(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenEntityWasRemoved()
    {
        // Arrange
        var entity = new Entity(99);

        var sut = new EntityMetadataStore();

        sut.Set(
            entity,
            () => EntityMetadata.Default(),
            existing => existing);

        sut.Remove(entity);

        // Act
        var result = sut.Contains(entity);

        // Assert
        Assert.False(result);
    }
}
