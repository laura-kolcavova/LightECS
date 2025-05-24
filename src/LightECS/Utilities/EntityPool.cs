using LightECS.Utilities.Abstractions;

namespace LightECS.Utilities;

internal sealed class EntityPool :
    IEntityPool
{
    private readonly Func<Entity> _factory;

    private readonly Stack<uint> _entityStack;

    private readonly object _lock = new();

    public EntityPool(
        Func<Entity> factory,
        int initialCapacity)
    {
        _factory = factory;

        _entityStack = new Stack<uint>(
            initialCapacity);
    }

    public EntityPool(
       Func<Entity> factory)
       : this(
           factory,
           0)
    {
    }

    public int Count => _entityStack.Count;

    public Entity Get()
    {
        lock (_lock)
        {
            if (_entityStack.TryPop(out var value))
            {
                var entity = new Entity(value);

                return entity;
            }
        }

        return _factory.Invoke();
    }

    public void Return(
        Entity entity)
    {
        lock (_lock)
        {
            if (_entityStack.Contains(entity.Id))
            {
                throw new InvalidOperationException("The entity has already been returned to the pool.");
            }

            _entityStack.Push(entity.Id);
        }
    }

    public bool Contains(
        Entity entity)
    {
        return _entityStack.Contains(entity.Id);
    }
}
