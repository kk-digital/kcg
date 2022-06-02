using UnityEngine;
using Enums;

public class ProjectileTest : MonoBehaviour
{
    // Vehilce Draw System
    Projectile.DrawSystem projectileDrawSystem;

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
        projectileDrawSystem = new Projectile.DrawSystem();

        // Loading Image
        projectileDrawSystem.Initialize(contexts, "Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades4.png", 16, 16, transform, Material,
            ProjectileType.Grenade, ProjectileDrawType.Standard);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        projectileDrawSystem.Draw();
    }
}
