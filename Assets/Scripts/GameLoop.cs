using UnityEngine;
using Enums;
using TileProperties;
public class GameLoop : MonoBehaviour
{
    [SerializeField] private Material material;

    private Agent.List agents;
    
    private const int FPS = 60;
    public TilePropertiesManager TilePropertiesManager;
    // Method for setting everything up, for like init GameManager for example
    private void Init()
    {

        //check if SceneManager even exists
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);

        }
        
        Application.targetFrameRate = FPS; // Cap at 60 FPS

        Agent.SpawnerSystem.Instance.SpawnPlayer(material);
        agents = new Agent.List();
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
        ECSInput.ProcessSystem.Instance.Update(ref agents);
        Agent.ProcessVelocitySystem.Instance.Process(ref agents);
    }

    // Method for Drawing
    private void Update()
    {
        Agent.DrawSystem.Instance.Draw(ref agents);
    }
}
