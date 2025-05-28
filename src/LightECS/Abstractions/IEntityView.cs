using LightECS.Events;

namespace LightECS.Abstractions;

public interface IEntityView
{
    public event EntityAddedEventHandler? EntityAdded;

    public event EntityRemovedEventHandler? EntityRemoved;

    public IEnumerable<Entity> AsEnumerable();
}

