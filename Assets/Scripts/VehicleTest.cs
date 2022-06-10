using UnityEngine;
using Entitas;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;

    // Vehicle Collision System
    Vehicle.ProcessCollisionSystem vehicleCollisionSystem;

    // Vehicle Spawner System
    Vehicle.SpawnerSystem vehicleSpawnerSystem;

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
        // Initialize Vehicle Spawner System
        vehicleSpawnerSystem = new Vehicle.SpawnerSystem();

        // Initialize Vehicle Physics System
        vehiclePhysics = new Vehicle.ProcessVelocitySystem();

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new Vehicle.DrawSystem();

        // Initialize Vehicle Collision System
        vehicleCollisionSystem = new Vehicle.ProcessCollisionSystem();

        // Loading Image
        vehicleSpawnerSystem.SpawnVehicle("Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96, Material, new Vector2(-5.0f, 0));

        // Initialize Planet Tile Map
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;
    }
    
    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {

        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        // Update Gravity
        if (canUpdateGravity)
            vehiclePhysics.UpdateGravity(Contexts.sharedInstance);

        // Update Collision Physics
        vehicleCollisionSystem.Update(tileMap);


        // Draw Vehicle
        vehicleDrawSystem.Draw(Instantiate(Material), transform, 12);

    }
}
