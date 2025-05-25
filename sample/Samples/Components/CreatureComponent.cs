using LightECS.Abstractions;

namespace Samples.Components;

internal sealed record CreatureComponent
    : IComponent
{
    public required string Name { get; init; }
}
