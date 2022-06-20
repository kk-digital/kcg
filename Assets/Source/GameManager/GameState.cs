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
    
    #region Tile

    public static readonly Tile.TileAtlasManager TileSpriteAtlasManager;
    public static readonly Tile.CreationApi TileCreationApi;

    #endregion
    
    #region Sprites

    public static readonly Sprites.SpriteAtlasManager SpriteAtlasManager;
    public static readonly Sprites.SpriteLoader SpriteLoader;

    #endregion

    #region Agent

    public static readonly Agent.SpawnerSystem SpawnerSystem;
    public static readonly Agent.AgentDrawSystem AgentDrawSystem;
    public static readonly Agent.EnemyAiSystem EnemyAiSystem;

    #endregion

    #region
    public static readonly Physics.MovableSystem MovableSystem;
    public static readonly Physics.ProcessCollisionSystem ProcessCollisionSystem;
    #endregion

    #region Inventory
    public static readonly Inventory.ManagerSystem InventoryManagerSystem;
    public static readonly Inventory.DrawSystem InventoryDrawSystem;
    #endregion

    public static readonly Item.SpawnerSystem ItemSpawnSystem;
    public static readonly Item.DrawSystem ItemDrawSystem;

    #region FloatingText
    public static readonly FloatingText.UpdateSystem FloatingTextUpdateSystem;
    public static readonly FloatingText.SpawnerSystem FloatingTextSpawnerSystem;
    public static readonly FloatingText.FloatingTextDrawSystem FloatingTextDrawSystem;
    #endregion

    public static readonly Utility.FileLoadingManager FileLoadingManager;
    public static readonly ECSInput.ProcessSystem ProcessSystem;

    static GameState()
    {
        Contexts entitasContext = Contexts.sharedInstance;

        SpriteLoader = new Sprites.SpriteLoader();
        TileSpriteAtlasManager = new Tile.TileAtlasManager(SpriteLoader);
        SpriteAtlasManager = new Sprites.SpriteAtlasManager(SpriteLoader);
        TileCreationApi = new Tile.CreationApi();
        FileLoadingManager = new Utility.FileLoadingManager();
        ProcessSystem = new ECSInput.ProcessSystem();
        SpawnerSystem = new Agent.SpawnerSystem();
        MovableSystem = new Physics.MovableSystem();
        AgentDrawSystem = new Agent.AgentDrawSystem();
        InventoryDrawSystem = new Inventory.DrawSystem();
        InventoryManagerSystem = new Inventory.ManagerSystem();
        ProcessCollisionSystem = new Physics.ProcessCollisionSystem();
        EnemyAiSystem = new Agent.EnemyAiSystem();
        AnimationManager = new Animation.AnimationManager();
        FloatingTextUpdateSystem = new FloatingText.UpdateSystem();
        FloatingTextSpawnerSystem = new FloatingText.SpawnerSystem(entitasContext);
        FloatingTextDrawSystem = new FloatingText.FloatingTextDrawSystem();
        AnimationUpdateSystem = new Animation.UpdateSystem();
        //UnityImage2DCache = new Sprites.UnityImage2DCache();
        ItemSpawnSystem = new Item.SpawnerSystem(entitasContext);
        ItemDrawSystem = new Item.DrawSystem(entitasContext);

    }
}
