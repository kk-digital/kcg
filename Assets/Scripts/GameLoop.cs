using UnityEngine;
using Enums;

public class GameLoop : MonoBehaviour
{
    private const int FPS = 60;

    // Method for setting everything up, for like init GameManager for example
    private void Init()
    {
        SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        Application.targetFrameRate = FPS; // Cap at 60 FPS
    }
    
    private void LoadAssets()
    {
    }

    private void Start()
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
