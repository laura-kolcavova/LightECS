using LightECS;

namespace TheConsoleWitcher.Components;

public sealed record CombatComponent
{
    public float Damage { get; init; } = 0;

    public float Armor { get; init; } = 0;

    public Entity? AttackedEntity { get; init; }

    public bool IsAttacking => AttackedEntity is not null;

    public CombatComponent Attack(Entity entity)
    {
        return this with
        {
            AttackedEntity = entity
        };
    }

    public CombatComponent StopAttack()
    {
        if (!IsAttacking)
        {
            return this;
        }

        return this with
        {
            AttackedEntity = null
        };
    }
}
