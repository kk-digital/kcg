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
    public static readonly Action.ActionManager ActionManager;
    public static readonly Action.ActionSchedulerSystem ActionSchedulerSystem;
    #endregion

    #region Tile

    public static readonly Tile.TileAtlasManager TileSpriteAtlasManager;
    public static readonly Tile.TileCreationApi TileCreationApi;

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
    public static readonly Inventory.InventoryManager InventoryManager;
    public static readonly Inventory.DrawSystem InventoryDrawSystem;
    #endregion

    #region Item
    public static readonly Item.SpawnerSystem ItemSpawnSystem;
    public static readonly Item.DrawSystem ItemDrawSystem;
    public static readonly Item.PickUpSystem ItemPickUpSystem;
    #endregion

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
        TileCreationApi = new Tile.TileCreationApi();
        FileLoadingManager = new Utility.FileLoadingManager();
        ProcessSystem = new ECSInput.ProcessSystem();
        SpawnerSystem = new Agent.SpawnerSystem();
        MovableSystem = new Physics.MovableSystem();
        AgentDrawSystem = new Agent.AgentDrawSystem();
        InventoryDrawSystem = new Inventory.DrawSystem();
        InventoryManager = new Inventory.InventoryManager();
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
        ItemPickUpSystem = new Item.PickUpSystem(entitasContext);
        ActionManager = new Action.ActionManager();
        ActionSchedulerSystem = new Action.ActionSchedulerSystem();
    }
}
