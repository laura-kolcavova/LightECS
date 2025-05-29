namespace TheConsoleWitcher.Components;

internal sealed record HealthComponent
{
    public float Health { get; init; } = 100;

    public float MaxHealth { get; init; } = 100;

    public bool HasNoHealth => Health <= 0;

    public HealthComponent TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return this;
        }

        var newHealth = Math.Max(
            Health - damage,
            0);

        return this with
        {
            Health = newHealth
        };
    }
}
