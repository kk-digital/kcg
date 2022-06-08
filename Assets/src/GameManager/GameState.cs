/// <summary>
/// <a href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors">Static Constructor</a>
/// </summary>
public class GameState
{
    public static readonly Contexts EntitasContext;
    public static readonly TileSpriteAtlas.TileSpriteAtlasManager TileSpriteAtlasManager;
    public static readonly SpriteAtlas.SpriteAtlasManager SpriteAtlasManager;
    public static readonly TileProperties.TileCreationApi TileCreationApi;
    public static readonly TileSpriteLoader.TileSpriteLoader TileSpriteLoader;
    public static readonly SpriteLoader.SpriteLoader SpriteLoader;
    public static readonly ImageLoader.FileLoadingManager FileLoadingManager;
    public static readonly ECSInput.ProcessSystem ProcessSystem;
    public static readonly Agent.SpawnerSystem SpawnerSystem;
    public static readonly Agent.MovableSystem MovableSystem;
    public static readonly Agent.DrawSystem DrawSystem;
    public static readonly Agent.CollisionSystem CollisionSystem;

    static GameState()
    {
        EntitasContext = Contexts.sharedInstance;
        TileSpriteAtlasManager = new TileSpriteAtlas.TileSpriteAtlasManager();
        SpriteAtlasManager = new SpriteAtlas.SpriteAtlasManager();
        TileCreationApi = new TileProperties.TileCreationApi();
        TileSpriteLoader = new TileSpriteLoader.TileSpriteLoader();
        SpriteLoader = new SpriteLoader.SpriteLoader();
        FileLoadingManager = new ImageLoader.FileLoadingManager();
        ProcessSystem = new ECSInput.ProcessSystem(EntitasContext);
        SpawnerSystem = new Agent.SpawnerSystem(EntitasContext);
        MovableSystem = new Agent.MovableSystem(EntitasContext);
        DrawSystem = new Agent.DrawSystem(EntitasContext);
        CollisionSystem = new Agent.CollisionSystem(EntitasContext);
    }
}
