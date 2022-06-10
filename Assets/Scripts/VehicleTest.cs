using UnityEngine;
using Entitas;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;

    // Vehicle Collision System
    Vehicle.ProcessCollisionSystem vehicleCollisionSystem;

    // Vehicle Physics
    public Vehicle.ProcessVelocitySystem vehiclePhysics;

    // Planet Tile Map
    private Planet.TileMap tileMap;

    // Rendering Material
    [SerializeField]
    Material Material;

    public bool canUpdateGravity = true;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        // Initialize Vehicle Draw System
        vehicleDrawSystem = new Vehicle.DrawSystem();

        // Initialize Vehicle Physics System
        vehiclePhysics = new Vehicle.ProcessVelocitySystem();

        // Initialize Vehicle Collision System
        vehicleCollisionSystem = new Vehicle.ProcessCollisionSystem();

        // Initialize Planet Tile Map
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Loading Image
        vehicleDrawSystem.Initialize(Contexts.sharedInstance, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96, transform, Material);
    }
    
    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Draw Vehicle
        vehicleDrawSystem.Draw();

        // Update Collision Physics
        vehicleCollisionSystem.Update(tileMap);

        // Update Gravity
        if(canUpdateGravity)
            vehiclePhysics.UpdateGravity(Contexts.sharedInstance);
    }
}
