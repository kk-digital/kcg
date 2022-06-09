using UnityEngine;
using Entitas;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;
    public Vehicle.ProcessVelocitySystem vehiclePhysics;

    // Rendering Material
    [SerializeField]
    Material Material;

    public bool canUpdateGravity = true;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new Vehicle.DrawSystem();
        vehiclePhysics = new Vehicle.ProcessVelocitySystem();

        // Loading Image
        vehicleDrawSystem.Initialize(Contexts.sharedInstance, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96, transform, Material);
    }
    
    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Draw Vehicle
        vehicleDrawSystem.Draw();

        // Update Collision Physics
        //vehicleDrawSystem.UpdateCollision();
        if(canUpdateGravity)
            vehiclePhysics.UpdateGravity(Contexts.sharedInstance);
    }
}
