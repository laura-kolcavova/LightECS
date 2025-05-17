namespace LightECS.Abstractions;

public interface IEntityStore
{
    public int Count { get; }

    public void Add(
        Entity entity);

    public bool Contains(
        Entity entity);

    public bool Remove(
        Entity entity);

    public void Clear();

    public IEnumerable<Entity> AsEnumerable();
}
