using UnityEngine;
using Enums;
using Entitas;

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
    int image;
    bool init;

    // Projectile Properties
    private Vector2 startPos;
    Vector3 worldPosition;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Initialize Projectile Draw System
        projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        projectileVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Initialize Projectile Spawner System
        projectileSpawnerSystem = new Projectile.SpawnerSystem();

        // Initialize Image
        image = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades7.png", 16, 16);

        init = true;
    }

    private void SpawnProjectile(Vector2 startPos)
    {
        // Loading Image
        projectileSpawnerSystem.SpawnProjectile(Material, image, 16, 16, startPos,
            ProjectileType.Grenade, ProjectileDrawType.Standard);
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        if(init)
        {
            // Clear last frame
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            // Get Vehicle Entites
            IGroup<GameEntity> entities =
            Contexts.sharedInstance.game.GetGroup(GameMatcher.AgentPlayer);
            foreach (var entity in entities)
            {
                startPos = entity.physicsPosition2D.Value;
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                SpawnProjectile(startPos);
            }

            Vector3 difference = new Vector2(worldPosition.x, worldPosition.y) - startPos;

            // Process Physics
            projectileVelocitySystem.Update(difference, Contexts.sharedInstance);

            // Draw Initialized Projectile
            projectileDrawSystem.Draw(Instantiate(Material), transform, 12);
        }
    }
}
