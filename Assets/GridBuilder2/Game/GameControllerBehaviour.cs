using UnityEngine;


public class GameControllerBehaviour : MonoBehaviour
{
    public ScriptableGameConfig gameConfig;

    GameController _gameController;

    void Awake() => _gameController = new GameController(Contexts.sharedInstance, gameConfig);
    void Start() => _gameController.Initialize();
    void Update() => _gameController.Execute();
}
