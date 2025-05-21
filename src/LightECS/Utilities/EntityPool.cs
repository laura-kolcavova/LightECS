namespace LightECS.Utilities;

internal sealed class EntityPool
{
    private readonly Func<Entity> _factory;

    private readonly Stack<Entity> _stack;

    private readonly object _lock = new();

    public EntityPool(
        Func<Entity> factory,
        int initialCapacity)
    {
        _factory = factory;

        _stack = new Stack<Entity>(
            initialCapacity);
    }

    public EntityPool(
       Func<Entity> factory)
       : this(
           factory,
           0)
    {
    }

    public int Count => _stack.Count;

    public Entity Get()
    {
        lock (_lock)
        {
            if (_stack.TryPop(out var value))
            {
                return value;
            }
        }

        return _factory.Invoke();
    }

    public void Return(
        Entity value)
    {
        lock (_lock)
        {
            if (_stack.Contains(value))
            {
                throw new InvalidOperationException("The entity has already been returned to the pool.");
            }

            _stack.Push(value);
        }
    }

    public bool Contains(Entity value)
    {
        return _stack.Contains(value);
    }
}
