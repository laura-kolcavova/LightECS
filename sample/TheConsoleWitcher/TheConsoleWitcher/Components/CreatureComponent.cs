using LightECS.Abstractions;

namespace TheConsoleWitcher.Components;

internal sealed record CreatureComponent
    : IComponent
{
    public required string Name { get; init; }
}
