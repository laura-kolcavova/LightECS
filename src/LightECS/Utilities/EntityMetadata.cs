namespace LightECS.Utilities;

internal sealed record EntityMetadata
{
    public required ComponentFlags ComponentFlags { get; init; }
}
