namespace LightECS.Utilities;

internal sealed record EntityMetadata
{
    public required ComponentFlags ComponentFlags { get; init; }

    internal static EntityMetadata Default()
    {
        return new EntityMetadata
        {
            ComponentFlags = ComponentFlags.None()
        };
    }
}
