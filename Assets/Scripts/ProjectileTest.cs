using UnityEngine;
using Enums;
using Entitas;
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
    private Vector2 startPos;
    Vector3 worldPosition;
    private Planet.TileMap tileMap;
    Vector3 difference;
    private Vector2 projectilePosition;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;

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
                difference = new Vector2(worldPosition.x, worldPosition.y) - startPos;

                Cell start = new Cell
                {
                    x = (int)startPos.x,
                    y = (int)startPos.y
                };

                Cell end = new Cell
                {
                    x = (int)projectilePosition.x,
                    y = (int)projectilePosition.y
                };

                // Spawn Projectile
                SpawnProjectile(startPos);

                // Log places drawed line go through
                //foreach (var cell in start.LineTo(end))
                //{
                //    ref var tile = ref tileMap.GetTileRef(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                //    if (tile.Type >= 0)
                //    {
                //        tileMap.RemoveTile(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                //        tileMap.BuildLayerTexture(Enums.Tile.MapLayerType.Front);
                //        IGroup<GameEntity> Centities = Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectileCollider);
                //        foreach (var entity in Centities)
                //        {
                //            entity.projectileCollider.isFirstSolid = true;
                //        }
                //    }
                //}

                Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y), Color.red);
            }

            // Process Physics
            projectileVelocitySystem.Update(difference, Contexts.sharedInstance);
            projectileCollisionSystem.Update(tileMap);

            // Draw Initialized Projectile
            projectileDrawSystem.Draw(Instantiate(Material), transform, 12);
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        var group = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectilePhysicsState2D));

        Gizmos.color = Color.green;

        foreach (var entity in group)
        {
            var pos = entity.projectilePhysicsState2D;
            var boxCollider = entity.physicsBox2DCollider;
            var boxBorders = boxCollider.CreateEntityBoxBorders(pos.Position);

            Gizmos.DrawWireCube(boxBorders.Center, new Vector3(boxCollider.Size.x, boxCollider.Size.y, 0.0f));
        }
    }
#endif
}
