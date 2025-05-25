using LightECS;
using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Factories;
using TheConsoleWitcher.Messages;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class CombatSystem
    : IUpdateSystem
{
    private readonly IEntityContext _entityContext;

    private readonly MessageFactory _messageFactory;

    private readonly IComponentStore<CreatureComponent> _creatureStore;

    private readonly IComponentStore<HealthComponent> _healthStore;

    private readonly IComponentStore<CombatComponent> _combatStore;

    public CombatSystem(
        IEntityContext entityContext,
        MessageFactory messageFactory)
    {
        _entityContext = entityContext;

        _messageFactory = messageFactory;

        _creatureStore = entityContext.UseStore<CreatureComponent>();
        _healthStore = entityContext.UseStore<HealthComponent>();
        _combatStore = entityContext.UseStore<CombatComponent>();
    }

    public void Update()
    {
        var entities = _entityContext
            .UseQuery()
            .With<CreatureComponent>()
            .With<HealthComponent>()
            .With<CombatComponent>()
            .AsEnumerable()
            .ToList();

        foreach (var entity in entities)
        {
            var combatData = _combatStore.Get(entity);

            if (!combatData.IsAttacking)
            {
                continue;
            }

            var attacker = new Attacker(
                Entity: entity,
                CreatureData: _creatureStore.Get(entity),
                CombatData: combatData);

            var targetEntity = combatData.AttackedEntity!.Value;

            var target = new Target(
                Entity: targetEntity,
                CreatureData: _creatureStore.Get(targetEntity),
                HealthComponent: _healthStore.Get(targetEntity),
                CombatData: _combatStore.Get(targetEntity));

            ProcessCombat(
                attacker,
                target);
        }
    }

    private void ProcessCombat(
        Attacker attacker,
        Target target)
    {
        var damageDealt = Math.Max(
            attacker.CombatData.Damage - target.CombatData.Armor,
            0);

        var newTargetHealthData = target
            .HealthComponent
            .TakeDamage(damageDealt);

        _healthStore.Set(
            target.Entity,
            newTargetHealthData);

        _messageFactory.Create(
            new AttackedMessage(
                AttackerName: attacker.CreatureData.Name,
                TargetName: target.CreatureData.Name,
                DamageDealt: damageDealt));

        if (newTargetHealthData.HasNoHealth)
        {
            _combatStore.Set(
                attacker.Entity,
                attacker.CombatData.StopAttack());

            _combatStore.Set(
                target.Entity,
                target.CombatData.StopAttack());

            _messageFactory.Create(
                new KilledMessage(
                    AttackerName: attacker.CreatureData.Name,
                    TargetName: target.CreatureData.Name));
        }
    }

    private sealed record Attacker(
        Entity Entity,
        CreatureComponent CreatureData,
        CombatComponent CombatData);

    private sealed record Target(
        Entity Entity,
        CreatureComponent CreatureData,
        HealthComponent HealthComponent,
        CombatComponent CombatData);
}
