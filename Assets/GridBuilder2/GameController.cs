using System;
using Entitas;


public class GameController
{
    readonly Systems _systems;

    public GameController(Contexts contexts, IGameConfig gameConfig)
    {
        var random = new Random(DateTime.UtcNow.Millisecond);
        UnityEngine.Random.InitState(random.Next());
        Rand.game = new Rand(random.Next());

        contexts.config.SetGameConfig(gameConfig);

        _systems = new GameSystems(contexts);
    }

    public void Initialize()
    {
        _systems.Initialize();
    }

    public void Execute()
    {
        _systems.Execute();
        _systems.Cleanup();
    }
}
