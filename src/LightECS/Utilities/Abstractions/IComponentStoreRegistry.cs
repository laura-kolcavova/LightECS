using LightECS.Abstractions;

namespace LightECS.Utilities.Abstractions;

internal interface IComponentStoreRegistry
{
    public IComponentStore<TComponent> Get<TComponent>()
        where TComponent : IComponent;

    public IComponentStore<TComponent> GetOrCreate<TComponent>()
      where TComponent : IComponent;

    public IReadOnlyCollection<IComponentStoreBase> GetAll();
}
