using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    // Projectile Draw System
    Projectile.DrawSystem projectileDrawSystem;
    Projectile.ProcessVelocitySystem processVelocitySystem;
    

    // Rendering Material
    [SerializeField]
    Material Material;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {

        // Initialize Projectile Draw System
        projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        processVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Loading Image
        projectileDrawSystem.Initialize(Contexts.sharedInstance, "Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades4.png", 16, 16, transform, Material,
            Enums.ProjectileType.Grenade, Enums.ProjectileDrawType.Standard);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Draw Initialized Projectile
        projectileDrawSystem.Draw();

        // Process Physics
        processVelocitySystem.Process(Contexts.sharedInstance);
    }
}
