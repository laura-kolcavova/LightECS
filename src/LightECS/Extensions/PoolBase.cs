namespace LightECS.Extensions;

public abstract class PoolBase<TValue>
    : IPool<TValue>
{
    private readonly Queue<TValue> _queue;

    protected PoolBase()
    {
        _queue = new Queue<TValue>();
    }

    protected PoolBase(
        int initialCapacity)
    {
        _queue = new Queue<TValue>(initialCapacity);
    }

    public TValue Get()
    {
        if (_queue.TryDequeue(out var value))
        {
            return value;
        }

        return Create();
    }

    public void Return(
        TValue value)
    {
        _queue.Append(value);
    }

    protected abstract TValue Create();
}
