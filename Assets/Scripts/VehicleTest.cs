using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Systems;
using TileSpriteLoader;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    VehicleDrawSystem vehicleDrawSystem;

    // Entitas's Contexts
    Contexts contexts;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new VehicleDrawSystem(contexts, "Assets\\StreamingAssets\\assets\\luis\\vehicles\\vintageFlyingCar.png", 32, 32);

        // Loading Image
        vehicleDrawSystem.Initialize();
    }
}
