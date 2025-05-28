namespace LightECS.Utilities;

internal readonly record struct EntityMetadata
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
