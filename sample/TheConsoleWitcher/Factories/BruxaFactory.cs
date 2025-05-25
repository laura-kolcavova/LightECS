using LightECS;
using LightECS.Abstractions;
using TheConsoleWitcher.Components;

namespace TheConsoleWitcher.Factories;

internal sealed class BruxaFactory
{
    private readonly IEntityContext _entityContext;

    public BruxaFactory(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
    }

    public Entity Create()
    {
        var entity = _entityContext.CreateEntity();

        _entityContext.Set(entity, new CreatureComponent
        {
            Name = "Bruxa",
        });

        _entityContext.Set(entity, new HealthComponent
        {
            Health = 300,
            MaxHealth = 300
        });

        _entityContext.Set(entity, new CombatComponent
        {
            Damage = 100,
            Armor = 25,
        });

        return entity;
    }
}
