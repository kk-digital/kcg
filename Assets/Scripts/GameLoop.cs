using UnityEngine;
using Enums;
using TileProperties;

using SystemView;

public class GameLoop : MonoBehaviour
{
    private const int FPS = 60;
    public TilePropertiesManager TilePropertiesManager;
    // Method for setting everything up, for like init GameManager for example

    public SystemState CurrentSystemState;

    private void Init()
    {

        //check if SceneManager even exists
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
        
        Application.targetFrameRate = FPS; // Cap at 60 FPS

        CurrentSystemState = new SystemState();
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
        
    }

    // Method for Drawing
    private void Update()
    {
        
    }
}
