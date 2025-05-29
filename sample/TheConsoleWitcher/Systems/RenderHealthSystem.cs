using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class RenderHealthSystem
    : IRenderSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<CreatureComponent> _creatureStore;

    private readonly IComponentStore<HealthComponent> _healthStore;

    private readonly IEntityView _entityView;

    public RenderHealthSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;

        _creatureStore = _entityContext.UseStore<CreatureComponent>();
        _healthStore = _entityContext.UseStore<HealthComponent>();

        _entityView = _entityContext
            .UseQuery()
            .With<CreatureComponent>()
            .With<HealthComponent>()
            .AsView();
    }

    public void Render()
    {
        foreach (var entity in _entityView.AsEnumerable())
        {
            var creatureData = _creatureStore.Get(entity);
            var healthData = _healthStore.Get(entity);

            Console.WriteLine($"{creatureData.Name}: {healthData.Health}/{healthData.MaxHealth} HP");
        }

        Console.WriteLine();
    }
}
