using LightECS.Abstractions;
using Samples.Messages.Abstractions;

namespace Samples.Components;

internal sealed record MessageComponent
    : IComponent
{
    public required IMessage Message { get; init; }

    public required long Timestamp { get; init; }
}
