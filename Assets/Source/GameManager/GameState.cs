using Agent;
using Item;
using Projectile;
/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    //public static readonly Sprites.UnityImage2DCache UnityImage2DCache;

    #region Atinmation
    public static readonly Animation.AnimationManager AnimationManager;
    public static readonly Animation.UpdateSystem AnimationUpdateSystem;
    #endregion

    #region Action
    public static readonly Action.ActionPropertyManager    ActionPropertyManager;
    public static readonly Action.ActionCreationSystem      ActionCreationSystem;
    public static readonly Action.ActionSchedulerSystem     ActionSchedulerSystem;
    public static readonly Action.InitializeSystem          ActionInitializeSystem;
    #endregion

    #region Tile

    public static readonly PlanetTileMap.TileAtlasManager TileSpriteAtlasManager;
    public static readonly PlanetTileMap.TileCreationApi TileCreationApi;

    #endregion
    
    #region Sprites

    public static readonly Sprites.SpriteAtlasManager SpriteAtlasManager;
    public static readonly Sprites.SpriteLoader SpriteLoader;

    #endregion

    #region Agent
    public static readonly Agent.AgentCreationApi AgentCreationApi;
    public static readonly Agent.AgentSpawnerSystem AgentSpawnerSystem;
    public static readonly Agent.AgentDrawSystem AgentDrawSystem;
    public static readonly Agent.EnemyAiSystem EnemyAiSystem;
    public static readonly Agent.AgentMeshBuilderSystem AgentMeshBuilderSystem;


    #endregion

    #region Physics
    public static readonly Physics.PhysicsMovableSystem PhysicsMovableSystem;
    public static readonly Physics.PhysicsProcessCollisionSystem PhysicsProcessCollisionSystem;
    #endregion

    #region Inventory
    public static readonly Inventory.InventoryManager InventoryManager;
    public static readonly Inventory.InventoryDrawSystem InventoryDrawSystem;
    #endregion

    #region Item
    public static readonly Item.SpawnerSystem ItemSpawnSystem;
    public static readonly Item.DrawSystem ItemDrawSystem;
    public static readonly Item.PickUpSystem ItemPickUpSystem;
    public static readonly Item.MeshBuilderSystem ItemMeshBuilderSystem;
    #endregion

    #region Projectile
    public static readonly Projectile.ProjectileCreationApi ProjectileCreationApi;
    public static readonly Projectile.ProcessCollisionSystem ProjectileCollisionSystem;
    public static readonly Projectile.MovementSystem ProjectileMovementSystem;
    public static readonly Projectile.SpawnerSystem ProjectileSpawnerSystem;
    public static readonly Projectile.DrawSystem ProjectileDrawSystem;
    public static readonly Projectile.MeshBuilderSystem ProjectileMeshBuilderSystem;
    #endregion

    #region FloatingText
    public static readonly FloatingText.FloatingTextUpdateSystem FloatingTextUpdateSystem;
    public static readonly FloatingText.FloatingTextSpawnerSystem FloatingTextSpawnerSystem;
    public static readonly FloatingText.FloatingTextDrawSystem FloatingTextDrawSystem;
    #endregion

    public static readonly Utility.FileLoadingManager FileLoadingManager;
    public static readonly ECSInput.InputProcessSystem InputProcessSystem;


    #region Particle
    public static readonly Particle.ParticleCreationApi ParticleCreationApi;
    public static readonly Particle.ParticleEmitterCreationApi ParticleEmitterCreationApi;
    public static readonly Particle.ParticleDrawSystem ParticleDrawSystem;
    public static readonly Particle.ParticleEmitterUpdateSystem ParticleEmitterUpdateSystem;
    public static readonly Particle.ParticleUpdateSystem ParticleUpdateSystem;
    public static readonly Particle.ParticleEmitterSpawnerSystem ParticleEmitterSpawnerSystem;
    public static readonly Particle.ParticleSpawnerSystem ParticleSpawnerSystem;
    public static readonly Particle.ParticleMeshBuilderSystem ParticleMeshBuilderSystem;

    #endregion

    static GameState()
    {
        SpriteLoader = new Sprites.SpriteLoader();
        TileSpriteAtlasManager = new PlanetTileMap.TileAtlasManager(SpriteLoader);
        SpriteAtlasManager = new Sprites.SpriteAtlasManager(SpriteLoader);
        TileCreationApi = new PlanetTileMap.TileCreationApi();
        FileLoadingManager = new Utility.FileLoadingManager();
        InputProcessSystem = new ECSInput.InputProcessSystem();
        AgentCreationApi = new Agent.AgentCreationApi();
        AgentSpawnerSystem = new Agent.AgentSpawnerSystem(AgentCreationApi);
        PhysicsMovableSystem = new Physics.PhysicsMovableSystem();
        AgentDrawSystem = new Agent.AgentDrawSystem();
        AgentMeshBuilderSystem = new Agent.AgentMeshBuilderSystem();
        InventoryDrawSystem = new Inventory.InventoryDrawSystem();
        InventoryManager = new Inventory.InventoryManager();
        PhysicsProcessCollisionSystem = new Physics.PhysicsProcessCollisionSystem();
        EnemyAiSystem = new Agent.EnemyAiSystem();
        AnimationManager = new Animation.AnimationManager();
        FloatingTextUpdateSystem = new FloatingText.FloatingTextUpdateSystem();
        FloatingTextSpawnerSystem = new FloatingText.FloatingTextSpawnerSystem();
        FloatingTextDrawSystem = new FloatingText.FloatingTextDrawSystem();
        AnimationUpdateSystem = new Animation.UpdateSystem();
        //UnityImage2DCache = new Sprites.UnityImage2DCache();
        ItemSpawnSystem = new Item.SpawnerSystem();
        ItemDrawSystem = new Item.DrawSystem();
        ItemPickUpSystem = new Item.PickUpSystem();
        ItemMeshBuilderSystem = new Item.MeshBuilderSystem();
        ActionPropertyManager = new Action.ActionPropertyManager();
        ActionCreationSystem = new Action.ActionCreationSystem();
        ActionSchedulerSystem = new Action.ActionSchedulerSystem();
        ActionInitializeSystem = new Action.InitializeSystem();
        ParticleCreationApi = new Particle.ParticleCreationApi();
        ParticleEmitterCreationApi = new Particle.ParticleEmitterCreationApi();
        ParticleDrawSystem = new Particle.ParticleDrawSystem();
        ParticleEmitterUpdateSystem = new Particle.ParticleEmitterUpdateSystem(ParticleEmitterCreationApi, ParticleCreationApi);
        ParticleMeshBuilderSystem = new Particle.ParticleMeshBuilderSystem();
        ParticleUpdateSystem = new Particle.ParticleUpdateSystem();
        ParticleEmitterSpawnerSystem = new Particle.ParticleEmitterSpawnerSystem(ParticleEmitterCreationApi, ParticleCreationApi);
        ParticleSpawnerSystem = new Particle.ParticleSpawnerSystem(ParticleCreationApi);
        ProjectileCreationApi = new Projectile.ProjectileCreationApi();
        ProjectileCollisionSystem = new Projectile.ProcessCollisionSystem();
        ProjectileMovementSystem = new Projectile.MovementSystem(ProjectileCreationApi);
        ProjectileSpawnerSystem = new Projectile.SpawnerSystem(ProjectileCreationApi);
        ProjectileDrawSystem = new Projectile.DrawSystem();
        ProjectileMeshBuilderSystem = new Projectile.MeshBuilderSystem();
    }
}
