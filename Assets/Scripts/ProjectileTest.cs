using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Systems;

public class ProjectileTest : MonoBehaviour
{
    // Vehilce Draw System
    ProjectileDrawSystem projectileDrawSystem;

    // Entitas's Contexts
    Contexts contexts;

    // Rendering Material
    [SerializeField]
    Material Material;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Assign Contexts
        contexts = Contexts.sharedInstance;

        // Initialize Vehicle Draw System
        projectileDrawSystem = new ProjectileDrawSystem(contexts, "Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades4.png", 16, 16, transform, Material,
            ProjectileType.Grenade, ProjectileDrawType.Standard);

        // Loading Image
        projectileDrawSystem.Initialize();
    }
}
