using UnityEngine;
using Entitas;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;
    Vehicle.ProcessVelocitySystem vehiclePhysics;

    // Entitas's Contexts
    Contexts contexts;

    // Rendering Material
    [SerializeField]
    Material Material;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new Vehicle.DrawSystem();
        vehiclePhysics = new Vehicle.ProcessVelocitySystem();

        // Loading Image
        vehicleDrawSystem.Initialize(contexts, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96, transform, Material);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Draw Vehicle
        vehicleDrawSystem.Draw();

        // Update Collision Physics
        //vehicleDrawSystem.UpdateCollision();
    }
}
