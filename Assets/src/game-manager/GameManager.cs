using Entitas;
using src.ecs.Game;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Systems ecsSystems;
    
    void Start()
    {
        ecsSystems = new GameFeatures(Contexts.sharedInstance);
        ecsSystems.Initialize();
    }

    void Update()
    {
        ecsSystems.Execute();
        ecsSystems.Cleanup();
    }

    void OnDestroy()
    {
        ecsSystems.TearDown();
        Contexts.sharedInstance = null;
    }
}
