namespace LightECS.Extensions;

public class Pool<TValue> :
    IPool<TValue>
{
    private readonly Func<TValue> _factory;

    private readonly Queue<TValue> _queue;

    public Pool(
        Func<TValue> factory)
    {
        _factory = factory;
        _queue = new Queue<TValue>();
    }

    public Pool(
        Func<TValue> factory,
        int initialCapacity)
    {
        _factory = factory;
        _queue = new Queue<TValue>(
            initialCapacity);
    }

    public TValue Get()
    {
        if (_queue.TryDequeue(out var value))
        {
            return value;
        }

        return _factory.Invoke();
    }

    public void Return(
        TValue value)
    {
        _queue.Append(value);
    }
}
