using LightECS.Abstractions;

namespace LightECS.Utilities.Abstractions;

internal interface IComponentStoreRegistry
{
    public IComponentStore<TComponent> Get<TComponent>();

    public IComponentStore<TComponent> GetOrCreate<TComponent>(
        out bool created);

    public IReadOnlyCollection<IComponentStoreBase> GetAll();
}
