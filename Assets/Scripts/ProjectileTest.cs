using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    // Projectile Draw System
    Projectile.DrawSystem projectileDrawSystem;
    Projectile.ProcessVelocitySystem processVelocitySystem;

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

        // Initialize Projectile Draw System
        projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        processVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Loading Image
        projectileDrawSystem.Initialize(contexts, "Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades4.png", 16, 16, transform, Material,
            Enums.ProjectileType.Grenade, Enums.ProjectileDrawType.Standard);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Draw Initialized Projectile
        projectileDrawSystem.Draw();

        // Process Physics
        processVelocitySystem.Process(contexts);
    }
}
