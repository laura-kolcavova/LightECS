using LightECS.Abstractions;
using TheConsoleWitcher.Messages.Abstractions;

namespace TheConsoleWitcher.Components;

internal sealed record MessageComponent
    : IComponent
{
    public required IMessage Message { get; init; }

    public required long Timestamp { get; init; }
}
