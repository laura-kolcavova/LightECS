using LightECS.Abstractions;
using Samples.Components;
using Samples.Messages;
using Samples.Systems.Abstractions;

namespace Samples.Systems;

internal sealed class RenderMessageSystem :
    IRenderSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<MessageComponent> _messageStore;

    public RenderMessageSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
        _messageStore = _entityContext.UseStore<MessageComponent>();
    }

    public void Render()
    {
        var messagesDataList = _entityContext
            .UseQuery()
            .With<MessageComponent>()
            .AsEnumerable()
            .Select(_messageStore.Get)
            .OrderBy(messageData => messageData.Timestamp)
            .ToList();

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
