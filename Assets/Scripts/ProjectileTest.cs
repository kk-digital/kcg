using UnityEngine;
using Enums;
using Entitas;
using Enums.Tile;
using KMath;
using Physics;

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
    Vector3 worldPosition;
    private Planet.TileMap tileMap;
    private Planet.ChunkList chunkList;
    Vec3f difference;
    private Vec2f projectilePosition;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        // Create Tile Map
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

        // Create Chunk List
        chunkList = tileMap.Chunks;

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

    // Spawn Projectiles
    private void SpawnProjectile(Vec2f startPos)
    {
        // Loading Image
        projectileSpawnerSystem.SpawnProjectile(Material, image, 16, 16, startPos,
            ProjectileType.Grenade, ProjectileDrawType.Standard);
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

            // Call Right Click Down Event
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Calculate cursor position
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                // Calculate difference
                var diff = new Vec2f(worldPosition.x, worldPosition.y) - startPos;
                difference = new Vec3f(diff.X, diff.Y);

                Cell start = new Cell
                {
                    x = (int)startPos.X,
                    y = (int)startPos.Y
                };

                Cell end = new Cell
                {
                    x = (int)projectilePosition.X,
                    y = (int)projectilePosition.Y
                };

                // Spawn Projectile
                SpawnProjectile(startPos);

                // Log Places Shooted Ray Go Through
                foreach (var cell in start.LineTo(end))
                {
                    // Get Chunks because it's faster
                    ref var tile = ref chunkList[cell.x, cell.y];
                    if (tile.Type is not (MapChunkType.Empty or MapChunkType.Error))
                    {
                        IGroup<GameEntity> cEntities = Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectileCollider);
                        foreach (var entity in cEntities)
                        {
                            entity.projectileCollider.isFirstSolid = true;
                        }
                    }
                }

                // Draw Debug Line to see shooted ray
                Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y), Color.red);
            }

            // Process Physics
            projectileVelocitySystem.Update(difference, Contexts.sharedInstance);

            // Process Collision System
            projectileCollisionSystem.Update(tileMap);

            // Draw Initialized Projectile
            projectileDrawSystem.Draw(Instantiate(Material), transform, 12);
        }
    }
}
