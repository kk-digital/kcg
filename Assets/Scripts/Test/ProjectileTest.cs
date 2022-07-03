using UnityEngine;
using Enums;
using Entitas;
using KMath;

public class ProjectileTest : MonoBehaviour
{
    // Projectile Physics System
    //Projectile.ProcessVelocitySystem projectileVelocitySystem;

    // Projectile Collision System
    Projectile.ProcessCollisionSystem projectileCollisionSystem;

    // Projectile Spawner System
    Projectile.SpawnerSystem projectileSpawnerSystem;

    // Projectile Mesh Builder System.
    Projectile.MeshBuilderSystem projectileMeshBuilderSystem;

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
        //projectileDrawSystem = new Projectile.DrawSystem();

        // Initialize Projectile Velocity System
        //projectileVelocitySystem = new Projectile.ProcessVelocitySystem();

        // Initialize Projectile Spawner System
        projectileSpawnerSystem = new Projectile.SpawnerSystem(GameState.ProjectileCreationApi);

        // Initialize Projectile Collision System
        projectileCollisionSystem = new Projectile.ProcessCollisionSystem();

        // Initialize Projectile Mesh Builder System.
        projectileMeshBuilderSystem = new Projectile.MeshBuilderSystem();
        projectileMeshBuilderSystem.Initialize(Material, transform, 12);

        // Initialize Image
        image = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Projectiles\\Grenades\\Grenade\\Grenades7.png", 16, 16);

        // Init is done, now all updates ready to work
        init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // check if the sprite atlas textures needs to be updated
        for (int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
        {
            GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // check if the tile sprite atlas textures needs to be updated
        for (int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
        {
            GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
        }

        if (init)
        {
            var test = Contexts.sharedInstance.agent.GetGroup(AgentMatcher.AgentPlayer);
            foreach (var entity in test)
            {
                startPos = entity.physicsPosition2D.Value;
            }

            IGroup<AgentEntity> Playerentities =
            Contexts.sharedInstance.agent.GetGroup(AgentMatcher.PhysicsPosition2D);
            foreach (var entity in Playerentities)
            {
                startPos = entity.physicsPosition2D.Value;
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
                projectileSpawnerSystem.SpawnProjectile(Contexts.sharedInstance, image, 16, 16, startPos,
                    start, end, ProjectileType.Grenade, ProjectileDrawType.Standard);
            }

            //projectileVelocitySystem.Update(new Vec3f(diff.X, diff.Y));

            // Process Collision System
            projectileCollisionSystem.Update(ref planetState.TileMap);

            // Draw Initialized Projectile
            projectileMeshBuilderSystem.UpdateMesh(Contexts.sharedInstance.projectile);
            Utility.Render.DrawFrame(ref projectileMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle));

        }
    }
}
