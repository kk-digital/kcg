/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    public static readonly Contexts EntitasContext;
    public static readonly Tile.SpriteAtlasManager TileSpriteAtlasManager;
    public static readonly Sprites.AtlasManager SpriteAtlasManager;
    public static readonly Tile.CreationApi CreationApi;
    public static readonly Sprites.Loader SpriteLoader;
    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    public static readonly ECSInput.ProcessSystem ProcessSystem;
    public static readonly Agent.SpawnerSystem SpawnerSystem;
    public static readonly Agent.MovableSystem MovableSystem;
    public static readonly Agent.DrawSystem DrawSystem;
    public static readonly Agent.CollisionSystem CollisionSystem;

    static GameState()
    {
        EntitasContext = Contexts.sharedInstance;
        TileSpriteAtlasManager = new Tile.SpriteAtlasManager();
        SpriteAtlasManager = new Sprites.AtlasManager();
        CreationApi = new Tile.CreationApi();
        SpriteLoader = new Sprites.Loader();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
        ProcessSystem = new ECSInput.ProcessSystem(EntitasContext);
        SpawnerSystem = new Agent.SpawnerSystem(EntitasContext);
        MovableSystem = new Agent.MovableSystem(EntitasContext);
        DrawSystem = new Agent.DrawSystem(EntitasContext);
        CollisionSystem = new Agent.CollisionSystem(EntitasContext);
    }
}
