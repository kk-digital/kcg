using UnityEngine;
using Enums;
using Entitas;
using KMath;

public class ProjectileTest : MonoBehaviour
{
    // Projectile Draw System
    Projectile.DrawSystem projectileDrawSystem;

    // Projectile Physics System
    Projectile.ProcessVelocitySystem projectileVelocitySystem;

    // Projectile Collision System
    Projectile.ProcessCollisionSystem projectileCollisionSystem;

    // Projectile Spawner System
    Projectile.SpawnerSystem projectileSpawnerSystem;

    // Rendering Material
    [SerializeField]
    Material Material;

    // Image
    private int image;

    // Initializtion state
    private bool init;

    // Projectile Properties
    private Vec2f startPos;
    Planet.PlanetState planetState;
    private Vec2f projectilePosition;
    private Vec2f worldPosition;
    private Vec2f diff;
    Cell start;
    Cell end;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Create Tile Map
        planetState = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().PlanetState;
        
        // Initialize Projectile Draw System
        projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        projectileVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Initialize Projectile Collision System
        projectileVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Initialize Projectile Spawner System
        projectileSpawnerSystem = new Projectile.SpawnerSystem();
        
        // Initialize Projectile Collision System
        projectileCollisionSystem = new Projectile.ProcessCollisionSystem();

        // Initialize Image
        image = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\grenades\\Grenades7.png", 16, 16);

        // Init is done, now all updates ready to work
        init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // check if the sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
        {
            GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // check if the tile sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
        {
            GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
        }

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
                startPos = new Vec2f(entity.physicsPosition2D.Value.Y, entity.physicsPosition2D.Value.Y);
            }

            IGroup<GameEntity> Pentities =
            Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectilePhysicsState2D);
            foreach (var entity in Pentities)
            {
                projectilePosition = entity.projectilePhysicsState2D.Position;
            }

            start = new Cell
            {
                x = (int)startPos.X,
                y = (int)startPos.Y
            };

            end = new Cell
            {
                x = (int)projectilePosition.X,
                y = (int)projectilePosition.Y
            };

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                worldPosition = new Vec2f(Camera.main.ScreenToWorldPoint(mousePos).x,
                    Camera.main.ScreenToWorldPoint(mousePos).y);

                diff = worldPosition - startPos;

                // Loading Image
                projectileSpawnerSystem.SpawnProjectile(Material, image, 16, 16, startPos,
                    start, end, ProjectileType.Grenade, ProjectileDrawType.Standard);
            }

            projectileVelocitySystem.Update(new Vec3f(diff.X, diff.Y), Contexts.sharedInstance);

            // Process Collision System
            projectileCollisionSystem.Update(ref planetState.TileMap);

            // Draw Initialized Projectile
            projectileDrawSystem.Draw(Instantiate(Material), transform, 12);
        }
    }
}
