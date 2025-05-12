namespace LightECS.Extensions;

public class Pool<TValue> :
    PoolBase<TValue>
{
    private readonly Func<TValue> _valueFactory;

    public Pool(
        Func<TValue> valueFactory)
        : base()
    {
        _valueFactory = valueFactory;
    }

    public Pool(
        Func<TValue> valueFactory,
        int initialCapacity)
        : base(initialCapacity)
    {
        _valueFactory = valueFactory;
    }

    protected override TValue Create()
    {
        return _valueFactory.Invoke();
    }
}
