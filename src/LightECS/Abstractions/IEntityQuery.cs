namespace LightECS.Abstractions;

public interface IEntityQuery
{
    public IEntityQuery With<TComponent>()
       where TComponent : IComponent;

    public IEntityView AsView();

    public IEnumerable<Entity> AsEnumerable();
}
