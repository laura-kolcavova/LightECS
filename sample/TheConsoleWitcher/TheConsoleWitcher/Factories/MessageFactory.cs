using LightECS;
using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Messages.Abstractions;

namespace TheConsoleWitcher.Factories;

internal sealed class MessageFactory
{
    private readonly IEntityContext _entityContext;

    public MessageFactory(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
    }

    public Entity Create(
        IMessage message)
    {
        var entity = _entityContext.CreateEntity();

        _entityContext.Set(entity, new MessageComponent
        {
            Message = message,
            Timestamp = DateTime.UtcNow.Ticks,
        });

        return entity;
    }
}
