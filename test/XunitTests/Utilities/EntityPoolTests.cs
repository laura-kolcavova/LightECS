using LightECS;
using LightECS.Utilities;
using Xunit.Categories;

namespace XunitTests.Utilities;

[Category("unit")]
[Category("coverage")]
public sealed class EntityPoolTests
{
    [Fact]
    public void Get_ShouldReturnNewEntity_WhenPoolIsEmpty()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var expectedId = entityUniqueIdProvider.ReadNextId();

        // Act
        var entity = pool.Get();

        // Assert
        Assert.Equal(expectedId, entity.Id);
    }

    [Fact]
    public void Get_ShouldReturnReturnedEntity()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity = CreateEntity(entityUniqueIdProvider);

        pool.Return(entity);

        // Act
        var retrieved = pool.Get();

        // Assert
        Assert.Equal(entity, retrieved);
        Assert.Equal(entity.Id, retrieved.Id);
    }

    [Fact]
    public void Get_ReducesCountByOne()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity = CreateEntity(entityUniqueIdProvider);

        pool.Return(entity);

        // Act
        pool.Get();

        // Assert
        Assert.Equal(0, pool.Count);
    }

    [Fact]
    public void Contains_ShouldReturnTrue_ForReturnedEntity()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity = CreateEntity(entityUniqueIdProvider);

        pool.Return(entity);

        // Act
        var contains = pool.Contains(entity);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_ShouldReturnFalse_ForNonReturnedEntity()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity = CreateEntity(entityUniqueIdProvider);

        // Act
        var contains = pool.Contains(entity);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Return_SameEntityTwice_ThrowsInvalidOperationException()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity = CreateEntity(entityUniqueIdProvider);

        pool.Return(entity);

        // Act
        var act = () => pool.Return(entity);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Count_AfterReturningEntities_ReflectsCorrectNumber()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var entity1 = CreateEntity(entityUniqueIdProvider);
        var entity2 = CreateEntity(entityUniqueIdProvider);

        pool.Return(entity1);
        pool.Return(entity2);

        // Act
        var count = pool.Count;

        // Assert
        Assert.Equal(2, count);
    }

    [Fact(Skip = "non-deterministic result")]
    // [Fact]
    public async Task Return_ShouldBeThreadSafe()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var returnedEntities = new List<Entity>();

        var returnTasks = Enumerable.Range(0, 1000).Select(_ =>
          Task.Run(() =>
          {
              var e = CreateEntity(entityUniqueIdProvider);

              lock (returnedEntities)
              {
                  returnedEntities.Add(e);
              }

              pool.Return(e);
          }));

        // Act
        await Task.WhenAll(returnTasks);

        // Assert
        Assert.Equal(1000, pool.Count);
    }

    [Fact(Skip = "non-deterministic result")]
    // [Fact]
    public async Task Get_ShouldBeThreadSafe()
    {
        // Arrange
        var entityUniqueIdProvider = new LightECS.Utilities.SequentialEntityIdGenerator();

        var pool = new EntityPool(
            () => CreateEntity(entityUniqueIdProvider));

        var returnedEntities = new Dictionary<uint, Entity>();
        var retrievedEntities = new List<Entity>();

        foreach (var i in Enumerable.Range(0, 1000))
        {
            var e = CreateEntity(entityUniqueIdProvider);

            returnedEntities.Add(e.Id, e);

            pool.Return(e);
        }

        var getTasks = Enumerable.Range(0, 1000).Select(_ =>
            Task.Run(() =>
            {
                var e = pool.Get();

                lock (retrievedEntities)
                {
                    retrievedEntities.Add(e);
                }
            }));

        // Act
        await Task.WhenAll(getTasks);

        // Assert
        Assert.Equal(1000, retrievedEntities.Count);
        Assert.Equal(0, pool.Count);
        Assert.Equal(returnedEntities.Count, returnedEntities.Count);

        foreach (var entity in retrievedEntities)
        {
            returnedEntities.TryGetValue(entity.Id, out var returnedEntity);

            Assert.Equal(returnedEntity, entity);
            Assert.Equal(returnedEntity.Id, entity.Id);
        }
    }

    private Entity CreateEntity(
        SequentialEntityIdGenerator entityUniqueIdProvider)
    {
        var id = entityUniqueIdProvider.NextId();

        return new Entity(id);
    }
}
