using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class ClearDeadCreatureSystem :
    IUpdateSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<HealthComponent> _healthStore;

    private readonly IEntityView _entityView;

    public ClearDeadCreatureSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;

        _healthStore = entityContext.UseStore<HealthComponent>();

        _entityView = entityContext
            .UseQuery()
            .With<HealthComponent>()
            .AsView();
    }

    public void Update()
    {
        foreach (var entity in _entityView.AsEnumerable())
        {
            var healthData = _healthStore.Get(entity);

            if (healthData.HasNoHealth)
            {
                _entityContext.DestroyEntity(entity);
            }
        }
    }
}
