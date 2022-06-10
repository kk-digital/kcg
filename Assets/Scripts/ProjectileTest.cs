using UnityEngine;
using Enums;
public class ProjectileTest : MonoBehaviour
{
    // Projectile Draw System
    Projectile.DrawSystem projectileDrawSystem;

    // Projectile Physics System
    Projectile.ProcessVelocitySystem projectileVelocitySystem;

    // Projectile Spawner System
    Projectile.SpawnerSystem projectileSpawnerSystem;

    // Rendering Material
    [SerializeField]
    Material Material;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Initialize Projectile Draw System
        projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        projectileVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Initialize Projectile Spawner System
        projectileSpawnerSystem = new Projectile.SpawnerSystem();

        // Loading Image
        projectileSpawnerSystem.SpawnProjectile("Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades4.png", 16, 16, Material, Vector2.zero, 
            ProjectileType.Grenade, ProjectileDrawType.Standard);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Clear last frame
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        // Process Physics
        projectileVelocitySystem.Process(Contexts.sharedInstance);

        // Draw Initialized Projectile
        projectileDrawSystem.Draw(Instantiate(Material), transform, 12);
    }
}
