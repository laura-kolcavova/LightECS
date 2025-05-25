using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class ClearDeadCreatureSystem :
    IUpdateSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<HealthComponent> _healthStore;

    public ClearDeadCreatureSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;

        _healthStore = entityContext.UseStore<HealthComponent>();
    }

    public void Update()
    {
        var entities = _entityContext
           .UseQuery()
           .With<HealthComponent>()
           .AsEnumerable()
           .ToList();

        foreach (var entity in entities)
        {
            var healthData = _healthStore.Get(entity);

            if (healthData.HasNoHealth)
            {
                _entityContext.DestroyEntity(entity);
            }
        }
    }
}
