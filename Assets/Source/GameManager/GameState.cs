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
    public static readonly PlanetTileMap.TilePropertyManager TilePropertyManager;

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

    #endregion

    #region Physics
    public static readonly Physics.PhysicsMovableSystem PhysicsMovableSystem;
    public static readonly Physics.PhysicsProcessCollisionSystem PhysicsProcessCollisionSystem;
    #endregion

    #region Inventory
    public static readonly Inventory.InventoryManager InventoryManager;
    public static readonly Inventory.DrawSystem InventoryDrawSystem;
    #endregion

    #region Item
    public static readonly Item.SpawnerSystem ItemSpawnSystem;
    public static readonly Item.DrawSystem ItemDrawSystem;
    public static readonly Item.PickUpSystem ItemPickUpSystem;
    #endregion

    #region Projectile
    public static readonly Projectile.ProcessCollisionSystem ProjectileCollisionSystem;
    public static readonly Projectile.MovementSystem ProjectileMovementSystem;
    public static readonly Projectile.SpawnerSystem ProjectileSpawnerSystem;
    public static readonly Projectile.DrawSystem ProjectileDrawSystem;
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
    #endregion

    static GameState()
    {
        Contexts entitasContext = Contexts.sharedInstance;

        SpriteLoader = new Sprites.SpriteLoader();
        TileSpriteAtlasManager = new PlanetTileMap.TileAtlasManager(SpriteLoader);
        TilePropertyManager = new PlanetTileMap.TilePropertyManager();
        SpriteAtlasManager = new Sprites.SpriteAtlasManager(SpriteLoader);
        TileCreationApi = new PlanetTileMap.TileCreationApi();
        FileLoadingManager = new Utility.FileLoadingManager();
        InputProcessSystem = new ECSInput.InputProcessSystem();
        AgentCreationApi = new Agent.AgentCreationApi();
        AgentSpawnerSystem = new Agent.AgentSpawnerSystem(AgentCreationApi);
        PhysicsMovableSystem = new Physics.PhysicsMovableSystem();
        AgentDrawSystem = new Agent.AgentDrawSystem();
        InventoryDrawSystem = new Inventory.DrawSystem();
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
        ActionPropertyManager = new Action.ActionPropertyManager();
        ActionCreationSystem = new Action.ActionCreationSystem();
        ActionSchedulerSystem = new Action.ActionSchedulerSystem();
        ActionInitializeSystem = new Action.InitializeSystem();
        ParticleCreationApi = new Particle.ParticleCreationApi();
        ParticleEmitterCreationApi = new Particle.ParticleEmitterCreationApi();
        ParticleDrawSystem = new Particle.ParticleDrawSystem();
        ParticleEmitterUpdateSystem = new Particle.ParticleEmitterUpdateSystem();
        ParticleUpdateSystem = new Particle.ParticleUpdateSystem();
        ParticleEmitterSpawnerSystem = new Particle.ParticleEmitterSpawnerSystem(ParticleEmitterCreationApi, ParticleCreationApi);
        ProjectileCollisionSystem = new Projectile.ProcessCollisionSystem();
        ProjectileMovementSystem = new Projectile.MovementSystem();
        ProjectileSpawnerSystem = new Projectile.SpawnerSystem();
        ProjectileDrawSystem = new Projectile.DrawSystem();
    }
}
