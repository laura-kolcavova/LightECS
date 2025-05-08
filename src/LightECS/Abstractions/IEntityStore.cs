namespace LightECS.Abstractions;

public interface IEntityStore
{
    public int Count { get; }

    public void Add(
        Entity entity);

    public bool Contains(
        Entity entity);

    public void Remove(
        Entity entity);

    public void RemoveAll();

    public IEnumerable<Entity> AsEnumerable();
}
