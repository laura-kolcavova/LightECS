using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Messages;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class RenderMessageSystem :
    IRenderSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<MessageComponent> _messageStore;

    private readonly IEntityView _entityView;

    public RenderMessageSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
        _messageStore = _entityContext.UseStore<MessageComponent>();

        _entityView = _entityContext
            .UseQuery()
            .With<MessageComponent>()
            .AsView();
    }

    public void Render()
    {
        var messagesDataList = _entityView
            .AsEnumerable()
            .Select(_messageStore.Get)
            .OrderBy(messageData => messageData.Timestamp);

        foreach (var messageData in messagesDataList)
        {
            if (messageData.Message is AttackedMessage attackedMessage)
            {
                RenderAttackedMessage(attackedMessage);
            }
            else if (messageData.Message is KilledMessage killedMessage)
            {
                RenderKilledMessage(killedMessage);
            }
        }
    }

    private void RenderAttackedMessage(AttackedMessage message)
    {
        Console.WriteLine(
            $"{message.AttackerName} attacked {message.TargetName} for {message.DamageDealt} damage!");
    }

    private void RenderKilledMessage(KilledMessage message)
    {
        Console.WriteLine(
            $"{message.AttackerName} killed {message.TargetName}!");
    }
}
