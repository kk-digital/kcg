/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    #region Tile

    public static readonly Tile.SpriteAtlasManager TileSpriteAtlasManager;
    public static readonly Tile.CreationApi TileCreationApi;

    #endregion
    
    #region Sprites

    public static readonly Sprites.AtlasManager SpriteAtlasManager;
    public static readonly Sprites.Loader SpriteLoader;

    #endregion

    #region Agent

    public static readonly Agent.SpawnerSystem SpawnerSystem;
    public static readonly Agent.DrawSystem DrawSystem;
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

    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    public static readonly ECSInput.ProcessSystem ProcessSystem;

    static GameState()
    {
        TileSpriteAtlasManager = new Tile.SpriteAtlasManager();
        SpriteAtlasManager = new Sprites.AtlasManager();
        TileCreationApi = new Tile.CreationApi();
        SpriteLoader = new Sprites.Loader();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
        ProcessSystem = new ECSInput.ProcessSystem();
        SpawnerSystem = new Agent.SpawnerSystem();
        MovableSystem = new Physics.MovableSystem();
        DrawSystem = new Agent.DrawSystem();
        InventoryDrawSystem = new Inventory.DrawSystem();
        InventoryManagerSystem = new Inventory.ManagerSystem();
        ProcessCollisionSystem = new Physics.ProcessCollisionSystem();
        EnemyAiSystem = new Agent.EnemyAiSystem();
    }
}
