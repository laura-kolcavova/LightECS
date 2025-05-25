using LightECS.Abstractions;
using Samples.Components;
using Samples.Systems.Abstractions;

namespace Samples.Systems;

internal sealed class RenderHealthSystem
    : IRenderSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IComponentStore<CreatureComponent> _creatureStore;
    private readonly IComponentStore<HealthComponent> _healthStore;

    public RenderHealthSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
        _creatureStore = _entityContext.UseStore<CreatureComponent>();
        _healthStore = _entityContext.UseStore<HealthComponent>();
    }

    public void Render()
    {
        var entities = _entityContext
            .UseQuery()
            .With<CreatureComponent>()
            .With<HealthComponent>()
            .AsEnumerable()
            .ToList();

        foreach (var entity in entities)
        {
            var creatureData = _creatureStore.Get(entity);
            var healthData = _healthStore.Get(entity);

            Console.WriteLine($"{creatureData.Name}: {healthData.Health}/{healthData.MaxHealth} HP");
        }

        Console.WriteLine();
    }
}
