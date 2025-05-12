namespace LightECS.Extensions;

public interface IPool<TValue>
{
    public TValue Get();

    public void Return(
        TValue value);
}
