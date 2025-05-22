namespace LightECS.Utilities.Abstractions;

internal interface IEntityMetadataStore
{
    public EntityMetadata Get(
       Entity entity);

    public void Set(
        Entity entity,
        EntityMetadata entityMetadata);

    public void Unset(
        Entity entity);
}
