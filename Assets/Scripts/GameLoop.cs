using UnityEngine;

using SystemView;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private Material material;

    private Agent.List agents;
    
    private const int FPS = 60;

    // Method for setting everything up, for like init GameManager for example

    public SystemState CurrentSystemState;

    private void Init()
    {

        //check if SceneManager even exists
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, Enums.SceneObjectType.SceneObjectTypeUtilityScript);
        }

        Application.targetFrameRate = FPS; // Cap at 60 FPS

        //Agent.SpawnerSystem.Instance.SpawnPlayer(material);
       // agents = new Agent.List();
    }
    
    private void LoadAssets()
    {
    }

    private void Awake()
    {
        Init();
        LoadAssets();
    }
    
    // Method to update physics
    private void FixedUpdate()
    {
        //ECSInput.ProcessSystem.Instance.Update(ref agents);
        //Agent.MovableSystem.Instance.CalculatePosition(ref agents);
    }

    // Method for Drawing
    private void Update()
    {
       // Agent.DrawSystem.Instance.Draw(ref agents);
    }
}
