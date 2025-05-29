using LightECS.Abstractions;
using TheConsoleWitcher.Components;
using TheConsoleWitcher.Systems.Abstractions;

namespace TheConsoleWitcher.Systems;

internal sealed class ClearMessageSystem
    : IUpdateSystem
{
    private readonly IEntityContext _entityContext;

    private readonly IEntityView _entityView;

    public ClearMessageSystem(
        IEntityContext entityContext)
    {
        _entityContext = entityContext;

        _entityView = _entityContext
            .UseQuery()
            .With<MessageComponent>()
            .AsView();
    }

    public void Update()
    {
        foreach (var entity in _entityView.AsEnumerable())
        {
            _entityContext.DestroyEntity(entity);
        }
    }
}
