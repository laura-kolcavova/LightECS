using LightECS;
using LightECS.Abstractions;
using Samples.Components;

namespace Samples.Factories;

internal sealed class GeraltFactory
{
    private readonly IEntityContext _entityContext;

    public GeraltFactory(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
    }

    public Entity Create()
    {
        var entity = _entityContext.CreateEntity();

        _entityContext.Set(entity, new CreatureComponent
        {
            Name = "Geralt",
        });

        _entityContext.Set(entity, new HealthComponent
        {
            Health = 500,
            MaxHealth = 500
        });

        _entityContext.Set(entity, new CombatComponent
        {
            Damage = 100,
            Armor = 50
        });

        return entity;
    }
}
