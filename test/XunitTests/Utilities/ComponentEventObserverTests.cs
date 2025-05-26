using LightECS;
using LightECS.Abstractions;
using LightECS.Events;
using LightECS.Utilities;
using LightECS.Utilities.Abstractions;
using NSubstitute;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class ComponentEventObserverTests
{
    public sealed record TestComponent : IComponent
    {
        public required string Name { get; init; }
    }

    [Fact]
    public void AttachStore_ShouldSubscribeToEvents()
    {
        // Arrange
        var componentStore = Substitute
            .For<IComponentStore<TestComponent>>();

        componentStore
            .When(x => x.ComponentAdded += Arg.Any<ComponentAddedEventHandler<TestComponent>>())
            .Do(_ => { });

        componentStore
            .When(x => x.ComponentRemoved += Arg.Any<ComponentRemovedEventHandler<TestComponent>>())
            .Do(_ => { });

        var flagIndexRegistry = Substitute
            .For<IComponentFlagIndexRegistry>();

        var entityMetadataStore = Substitute
            .For<IEntityMetadataStore>();

        var sut = new ComponentEventObserver<TestComponent>(
            flagIndexRegistry,
            entityMetadataStore);

        // Act
        sut.AttachStore(componentStore);

        // Assert
        componentStore
            .Received(1)
            .ComponentAdded += Arg.Any<ComponentAddedEventHandler<TestComponent>>();

        componentStore
            .Received(1)
            .ComponentRemoved += Arg.Any<ComponentRemovedEventHandler<TestComponent>>();
    }

    [Fact]
    public void DetachStore_ShouldUnsubscribeAndNullStore()
    {
        // Arrange
        var componentStore = Substitute
            .For<IComponentStore<TestComponent>>();

        componentStore
            .When(x => x.ComponentAdded -= Arg.Any<ComponentAddedEventHandler<TestComponent>>())
            .Do(_ => { });

        componentStore
            .When(x => x.ComponentRemoved -= Arg.Any<ComponentRemovedEventHandler<TestComponent>>())
            .Do(_ => { });

        var flagIndexRegistry = Substitute
            .For<IComponentFlagIndexRegistry>();

        var entityMetadataStore = Substitute
            .For<IEntityMetadataStore>();

        var sut = new ComponentEventObserver<TestComponent>(
            flagIndexRegistry,
            entityMetadataStore);

        sut.AttachStore(componentStore);

        // Act
        sut.DetachStore();

        // Assert
        componentStore
            .Received(1)
            .ComponentAdded -= Arg.Any<ComponentAddedEventHandler<TestComponent>>();

        componentStore
            .Received(1)
            .ComponentRemoved -= Arg.Any<ComponentRemovedEventHandler<TestComponent>>();
    }

    [Fact]
    public void HandleComponentAdded_ShouldSetComponentFlag()
    {
        // Arrange
        var componentStore = new ComponentStore<TestComponent>();

        var entity = new Entity(1);

        var component = new TestComponent { Name = "Test" };

        var flagIndexRegistry = new ComponentFlagIndexRegistry(64);

        var entityMetadataStore = new EntityMetadataStore(128);

        var sut = new ComponentEventObserver<TestComponent>(
            flagIndexRegistry,
            entityMetadataStore);

        sut.AttachStore(componentStore);

        // Act
        componentStore.Set(entity, component);

        // Assert
        Assert.Equal(
            ComponentFlags.FromIndex(1),
            entityMetadataStore.Get(entity).ComponentFlags);
    }

    [Fact]
    public void HandleComponentRemoved_ShouldUnsetComponentFlag()
    {
        // Arrange
        var componentStore = new ComponentStore<TestComponent>();

        var entity = new Entity(1);

        var component = new TestComponent { Name = "Test" };

        var flagIndexRegistry = new ComponentFlagIndexRegistry(64);

        var entityMetadataStore = new EntityMetadataStore(128);

        var sut = new ComponentEventObserver<TestComponent>(
            flagIndexRegistry,
            entityMetadataStore);

        sut.AttachStore(componentStore);

        componentStore.Set(entity, component);

        // Act
        componentStore.Unset(entity);

        // Assert
        Assert.Equal(
          ComponentFlags.None(),
          entityMetadataStore.Get(entity).ComponentFlags);
    }
}
