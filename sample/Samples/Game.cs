using LightECS;
using Samples.Components;
using Samples.Factories;
using Samples.Systems;
using Samples.Systems.Abstractions;

namespace Samples;

internal sealed class Game
{
    private readonly EntityContext _context;

    private readonly List<IUpdateSystem> _updateSystems;

    private readonly List<IRenderSystem> _renderSystems;

    public Game()
    {
        _context = new EntityContext();

        _updateSystems = [];
        _renderSystems = [];

    }
    public void Initialize()
    {
        var geraltFactory = new GeraltFactory(_context);
        var bruxaFactory = new BruxaFactory(_context);
        var messageFactory = new MessageFactory(_context);

        var geralt = geraltFactory.Create();
        var bruxa = bruxaFactory.Create();

        AddUpdateSystems([
            new ClearMessageSystem(_context),
            new ClearDeadCreatureSystem(_context),
            new CombatSystem(_context, messageFactory),
        ]);

        AddRenderSystems([
            new RenderHealthSystem(_context),
            new RenderMessageSystem(_context)
        ]);

        var geraltCombatData = _context.Get<CombatComponent>(geralt);
        var bruxaCombatData = _context.Get<CombatComponent>(bruxa);

        _context.Set(
            geralt,
            geraltCombatData.Attack(bruxa));

        _context.Set(
            bruxa,
            bruxaCombatData.Attack(geralt));
    }

    public void Run()
    {
        while (true)
        {
            Update();
            Render();

            Console.ReadLine();
        }
    }

    private void Update()
    {
        foreach (var updateSystem in _updateSystems)
        {
            updateSystem.Update();
        }
    }

    private void Render()
    {
        Console.Clear();

        foreach (var renderSystem in _renderSystems)
        {
            renderSystem.Render();
        }
    }

    private void AddUpdateSystems(
        IEnumerable<IUpdateSystem> updateSystems)
    {
        _updateSystems.AddRange(updateSystems);
    }

    private void AddRenderSystems(
        IEnumerable<IRenderSystem> renderSystems)
    {
        _renderSystems.AddRange(renderSystems);
    }
}
