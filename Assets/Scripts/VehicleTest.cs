using UnityEngine;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;

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
        vehicleDrawSystem = new Vehicle.DrawSystem(contexts, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\Jet_chassis.png", 144, 96, transform, Material);

        // Loading Image
        vehicleDrawSystem.Initialize();
    }
}
