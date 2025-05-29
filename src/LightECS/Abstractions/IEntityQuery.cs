namespace LightECS.Abstractions;

public interface IEntityQuery
{
    public IEntityQuery With<TComponent>();

    public IEntityView AsView();

    public IEnumerable<Entity> AsEnumerable();
}
