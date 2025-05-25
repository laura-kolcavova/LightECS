using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class ClearMessageSystem
    : IUpdateSystem
{
    private readonly IEntityContext _entityContext;

    public ClearMessageSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;
    }

    public void Update()
    {
        var entities = _entityContext
            .UseQuery()
            .With<MessageComponent>()
            .AsEnumerable()
            .ToList();

        foreach (var entity in entities)
        {
            _entityContext.DestroyEntity(entity);
        }
    }
}
