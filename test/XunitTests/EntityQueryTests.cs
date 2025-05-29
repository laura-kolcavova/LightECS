using LightECS;
using LightECS.Abstractions;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;
using NSubstitute;
using Xunit.Categories;

namespace XunitTests;

[Category("unit")]
[Category("coverage")]
public sealed class EntityQueryTests
{
    public sealed record TestComponent
    {
        public required string Name { get; init; }
    }

    [Fact]
    public void With_ShouldReturnNewQueryWithUpdatedFlags()
    {
        // Arrange
        var entityStore = Substitute.For<IEntityStore>();

        var metadataStore = Substitute.For<IEntityMetadataStore>();

        var flagRegistry = Substitute
            .For<IComponentFlagIndexRegistry>();

        flagRegistry
            .GetOrCreate<TestComponent>()
            .Returns((byte)1);

        var query = new EntityQuery(
            entityStore,
            metadataStore,
            flagRegistry);

        // Act
        var result = query.With<TestComponent>();

        // Assert
        Assert.NotSame(query, result);
    }

    [Fact]
    public void AsEnumerable_ShouldReturnAll_WhenEntityFlagsAndQueryFlagsAreEmpty()
    {
        // Arrange
        var entity1 = new Entity(1);
        var entity2 = new Entity(2);

        var entityStore = Substitute.For<IEntityStore>();

        entityStore
            .AsEnumerable()
            .Returns(new[]
            {
                entity1,
                entity2
            });

        var metadataStore = Substitute.For<IEntityMetadataStore>();

        metadataStore
            .TryGet(
                entity1,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = EntityMetadata.Default();
                return false;
            });

        metadataStore
            .TryGet(
                entity2,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = EntityMetadata.Default();
                return false;
            });

        var flagRegistry = Substitute
            .For<IComponentFlagIndexRegistry>();

        var query = new EntityQuery(
            entityStore,
            metadataStore,
            flagRegistry);

        // Act
        var result = query.AsEnumerable();

        // Assert
        Assert.Equivalent(result, new[] { entity1, entity2 });
    }

    [Fact]
    public void AsEnumerable_ShouldReturnMatchingEntities_ThatHaveAllComponentFlags()
    {
        // Arrange
        var entity1 = new Entity(1);
        var entity2 = new Entity(2);
        var entity3 = new Entity(3);

        var entityStore = Substitute.For<IEntityStore>();

        entityStore
            .AsEnumerable()
            .Returns(
                new[]
                {
                    entity1,
                    entity2,
                    entity3
                });

        var metadataStore = Substitute.For<IEntityMetadataStore>();

        byte flagIndex = 1;

        var matchingFlags = ComponentFlags.FromIndex(flagIndex);

        var nonMatchingFlags = ComponentFlags.None();

        metadataStore
            .TryGet(
                entity1,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = new EntityMetadata { ComponentFlags = matchingFlags };
                return true;
            });

        metadataStore
            .TryGet(
                entity2,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = new EntityMetadata { ComponentFlags = nonMatchingFlags };
                return true;
            });

        metadataStore
            .TryGet(
                entity3,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = new EntityMetadata { ComponentFlags = matchingFlags };
                return true;
            });

        var flagRegistry = Substitute
           .For<IComponentFlagIndexRegistry>();

        var query = new EntityQuery(
            entityStore,
            metadataStore,
            flagRegistry,
            matchingFlags);

        // Act
        var result = query.AsEnumerable();

        // Assert
        Assert.Equivalent(result, new[] { entity1, entity3 });
    }

    [Fact]
    public void AsEnumerable_ShouldUseDefaultMetadata_WhenTryGetReturnsFalse()
    {
        // Arrange
        var entity1 = new Entity(1);

        var entityStore = Substitute.For<IEntityStore>();

        entityStore
            .AsEnumerable()
            .Returns(
                new[]
                {
                    entity1
                });

        var metadataStore = Substitute.For<IEntityMetadataStore>();

        metadataStore
            .TryGet(
                entity1,
                out Arg.Any<EntityMetadata>())
            .Returns(x =>
            {
                x[1] = EntityMetadata.Default();
                return false;
            });

        var flagRegistry = Substitute
          .For<IComponentFlagIndexRegistry>();

        var queryFlags = ComponentFlags.FromIndex(1);

        var query = new EntityQuery(
            entityStore,
            metadataStore,
            flagRegistry,
            queryFlags);

        // Act
        var result = query.AsEnumerable();

        // Assert
        Assert.Empty(result);
    }
}