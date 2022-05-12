using UnityEngine;
using System.Collections;
using Entitas;


public class GameController : MonoBehaviour {
    Systems _systems;

    void Start()
    {
        Random.seed = 42;

        _systems = createSystems(Pools.pool);
        _systems.Initialize();
    }

    void Update()
    {
        _systems.Execute();
    }

    Systems createSystems(Pool pool)
    {
#if (UNITY_EDITOR)
        return new DebugSystems()
#else
        return new Systems()
#endif

            // Generate tile map
            .Add(pool.CreateSystem<GenerateMapSystem>());
    }
}
